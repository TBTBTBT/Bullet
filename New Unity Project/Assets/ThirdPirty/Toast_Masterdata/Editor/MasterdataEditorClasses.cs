using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Toast.Masterdata.Editor
{


    public class TableData
    {
        public Dictionary<string, (List<string> Data,string Define, int Width)> Data = new Dictionary<string, (List<string> Data, string Define, int Width)>();
        private string labelCache = "";
        private int selectedIndexCache = 0;
        public void AddData(string label,string type)
        {
            if (label != "" && !Data.ContainsKey(label))
            {
                Data.Add(label,( new List<string>() , type , 50 ));
            }

            CheckDataLength();
        }

        void RemoveData(string label)
        {
            if (label != "" && Data.ContainsKey(label))
            {
                Data.Remove(label);
               
            }

        }

        void AddRow()
        {
            foreach (var list in Data)
            {
                list.Value.Data.Add("");
            }

            CheckDataLength();
        }
        void RemoveRow(int row)
        {
            foreach (var list in Data)
            {
                list.Value.Data.RemoveAt(row);
            }

        }
        public void View()
        {

            GUILayout.BeginVertical(GUI.skin.box);
            //ラベル表示
            AddColumnButton();

            ViewData(Data);
            //カラム表示

            AddRowButton();
            GUILayout.EndVertical();
        }

        private void LabelRightClickEvent(string label, Rect rect)
        {
            var ev = Event.current;
            if (ev.type != EventType.MouseDown || ev.button != 1 || !rect.Contains(ev.mousePosition))
            {
                return;
            }

            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete " + label), false, () => { RemoveData(label); });
            menu.ShowAsContext();
            ev.Use();


        }

        private void CheckDataLength()
        {
            var max = 0;
            foreach (var list in Data)
            {
                max = Math.Max(list.Value.Data.Count ,max) ;
            }

            foreach (var list in Data)
            {
                var d = max - list.Value.Data.Count;
                if (d > 0)
                {
                    var l = new string[d];
                    list.Value.Data.AddRange(l);
                }
            }
        }

        private void ViewData(Dictionary<string, (List<string> Data, string Define, int Width)> data)
        {
            GUILayout.BeginHorizontal();
            
            foreach (var list in data)
            {
                GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(list.Value.Width));
                GUILayout.Label(list.Key, EditorStyles.miniButtonMid);
                //公式によるとリペイント時に限るらしいがこれでうまくいってるので…
                var rect = GUILayoutUtility.GetLastRect();
                LabelRightClickEvent(list.Key, rect);
               
                GUILayout.Label(list.Value.Define);
                for (var i = 0; i < list.Value.Data.Count; i++)
                {
                    list.Value.Data[i] = GUILayout.TextField(list.Value.Data[i]);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        //足りないところを埋める
        private void FillData()
        {

        }
        //列追加
        private void AddColumnButton()
        {
            labelCache = GUILayout.TextArea(labelCache);
            if (GUILayout.Button("+"))
            {
                AddData(labelCache,"string");
                labelCache = "";
            }

        }
        //要素追加
        private void AddRowButton()
        {
            if (GUILayout.Button("+"))
            {
                AddRow();
            }

        }
    }

    class TableToClass
    {

    }

    class TableToJson
    {

    }

}