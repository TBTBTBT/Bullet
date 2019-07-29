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
        public Dictionary<string, List<string>> Data = new Dictionary<string, List<string>>();
        string labelCache = "";
        private int selectedIndexCache = 0;
        public void AddData(string label)
        {
            if (label != "" && !Data.ContainsKey(label))
            {
                Data.Add(label, new List<string>());
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
                list.Value.Add("");
            }

            CheckDataLength();
        }
        void RemoveRow(int row)
        {
            foreach (var list in Data)
            {
                list.Value.RemoveAt(row);
            }

        }
        public void View()
        {

            GUILayout.BeginVertical(GUI.skin.box);
            //ラベル表示
            AddColumnButton();
            
            GUILayout.BeginHorizontal();
            ViewLabel(Data.Keys.ToArray()); 

            GUILayout.EndHorizontal();
            ViewData(Data.Values.ToArray());
            //カラム表示
            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();
            AddRowButton();
            GUILayout.EndVertical();
        }


        private void ViewLabel(string[] texts)
        {
            if (texts.Length <= 0)
            {
                return;
                selectedIndexCache =
                    GUILayout.SelectionGrid(selectedIndexCache, texts, texts.Length, EditorStyles.miniButtonMid);
            }

            foreach (var text in texts)
            {
                GUILayout.Label(text, EditorStyles.miniButtonMid);

                //公式によるとリペイント時に限るらしいがこれでうまくいってるので…
                var rect = GUILayoutUtility.GetLastRect();

                LabelRightClickEvent(text,rect);
                
            }
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
                max = Math.Max(list.Value.Count ,max) ;
            }

            foreach (var list in Data)
            {
                var d = max - list.Value.Count;
                if (d > 0)
                {
                    var l = new string[d];
                    list.Value.AddRange(l);
                }
            }
        }

        private void ViewData(List<string>[] data)
        {
            GUILayout.BeginHorizontal();
            foreach (var list in data)
            {
                GUILayout.BeginVertical(GUI.skin.box);
                for (var i = 0; i < list.Count; i++)
                {
                    list[i] = GUILayout.TextArea(list[i]);
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
                AddData(labelCache);
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