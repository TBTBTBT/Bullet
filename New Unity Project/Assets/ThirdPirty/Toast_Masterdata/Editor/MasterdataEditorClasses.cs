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
        public Dictionary<string, (List<string> Data,int Define, int Width)> Data = new Dictionary<string, (List<string> Data, int Define, int Width)>();
        private string labelCache = "";
        private int defineCache = 0;
        private int selectedIndexCache = 0;
        public void AddData(string label,int type)
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

            ViewData(Data);

            AddRowButton();
            AddSaveToJsonButton();
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
                    var l = Enumerable.Repeat<string>("", d).ToArray();

                    list.Value.Data.AddRange(l);
                }
            }
        }

        private void ViewData(Dictionary<string, (List<string> Data, int Define, int Width)> data)
        {
            GUILayout.BeginHorizontal();
            
            foreach (var list in data)
            {
                GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(list.Value.Width));
                GUILayout.Label(list.Key, EditorStyles.miniButtonMid);
                //公式によるとリペイント時に限るらしいがこれでうまくいってるので…
                var rect = GUILayoutUtility.GetLastRect();
                LabelRightClickEvent(list.Key, rect);
               
                GUILayout.Label(((MasterdataSetting.Define)list.Value.Define).ToString());

                for (var i = 0; i < list.Value.Data.Count; i++)
                {
                    list.Value.Data[i] = GUILayout.TextField(list.Value.Data[i]);
                }
                GUILayout.EndVertical();
            }
            AddColumnButton(100);
            GUILayout.EndHorizontal();
        }
        //足りないところを埋める
        private void FillData()
        {

        }

        private void PullDown(string[] types)
        {

        }
        //列追加
        private void AddColumnButton(int width)
        {
            GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(width));
           // GUILayout.BeginHorizontal();
            GUILayout.Label("name:");
            labelCache = GUILayout.TextField(labelCache);
          
            //GUILayout.EndHorizontal();
            //GUILayout.BeginHorizontal();
            GUILayout.Label("type:");
            defineCache = EditorGUILayout.Popup(defineCache, Enum.GetNames(typeof(MasterdataSetting.Define)));
            //GUILayout.EndHorizontal();
            if (GUILayout.Button("+"))
            {
                AddData(labelCache, defineCache);
                labelCache = "";
                defineCache = 0;
            }
            GUILayout.EndVertical();
        }
        //要素追加
        private void AddRowButton()
        {
            if (GUILayout.Button("+",GUILayout.Width(100)))
            {
                AddRow();
            }

        }
        private void AddSaveToJsonButton()
        {
            if (GUILayout.Button("SAVE AS", GUILayout.Width(100)))
            {
                SaveToJson();
            }

        }
        private void SaveToJson()
        {
            var max = 0;
            foreach (var l in Data)
            {
                max = Math.Max(l.Value.Data.Count, max);
            }
            var list = new List<Dictionary<string, string>>();
            
            for (int i = 0;i < max; i++)
            {
                var dic = new Dictionary<string, string>();
                foreach (var d in Data)
                {
                    dic.Add(d.Key, d.Value.Data[i]);

                }
                list.Add(dic);
            }
           
            
            TableToJson.MakeJson(Application.dataPath+ "/test.json",list);
        }

        private void SaveToClass()
        {

        }
    }

    class DefineToClass
    {
        public static void MakeScript(string name ,List<Dictionary<string, string>> def)
        {

            var json = MiniJSON.Json.Serialize(def);
        }
    }

    class TableToJson
    {
        public static void MakeJson(string path, List<Dictionary<string, string>> list)
        {
            var json = MiniJSON.Json.Serialize(list);
            Debug.Log(json);

        }
        public static void MakeDefJson()
        {

        }
    }

}