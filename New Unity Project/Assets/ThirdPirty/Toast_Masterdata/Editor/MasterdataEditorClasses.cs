﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Toast.Masterdata.Editor
{


    public class TableData
    {
        
        public Dictionary<string, (List<string> Data,Type Define, int Width)> Data = new Dictionary<string, (List<string> Data, Type Define, int Width)>();
        private List<Type> _typeList = new List<Type>();
        private string labelCache = "";
        private int defineCache = 0;
        private int selectedIndexCache = 0;
        private int selectedClassCache = 0;
        private List<Type> _classes = new List<Type>();
        public void AddData(string label,Type type)
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

        public void LoadClassList()
        {
            _classes.Clear();
            foreach (Type type in
                Assembly.GetAssembly(typeof(IMasterRecord)).GetTypes()
                    .Where(myType =>
                        myType.IsClass && typeof(IMasterRecord).IsAssignableFrom(myType)))
            {
                Debug.Log(type.ToString());
                _classes.Add(type);
            }
        }
        public void ViewClasses()
        {
            GUILayout.BeginVertical(GUI.skin.box,GUILayout.Width(400));

            selectedClassCache = EditorGUILayout.Popup("Class", selectedClassCache, _classes.ConvertAll(_=>_.ToString()).ToArray());
            if (GUILayout.Button("Go",GUILayout.Width(200)))
            {
                LoadFromClass(selectedClassCache);
            }
            GUILayout.EndVertical();
        }
        public void View()
        {

            GUILayout.BeginVertical(GUI.skin.box);

            ViewData(Data);

            AddRowButton();
            AddSaveToJsonButton();
            AddLoadButton();
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

        private void ViewData(Dictionary<string, (List<string> Data, Type Define, int Width)> data)
        {
            GUILayout.BeginHorizontal();
            
            foreach (var list in data)
            {
                GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(list.Value.Width));
                GUILayout.Label(list.Key, EditorStyles.miniButtonMid);
                //公式によるとリペイント時に限るらしいがこれでうまくいってるので…
                var rect = GUILayoutUtility.GetLastRect();
                LabelRightClickEvent(list.Key, rect);
               
                GUILayout.Label((list.Value.Define).ToString());
                Func<string, string> view = null;
                switch (list.Value.Define)
                {
                    default:
                        if (list.Value.Define.IsEnum)
                        {
                            view = str =>
                            {
                                var parse = 0;
                                int.TryParse(str, out parse);
                                return EditorGUILayout.Popup(parse, Enum.GetNames(list.Value.Define)).ToString();
                            };
                        }
                        else
                        {
                            view = str =>
                            {
                                return GUILayout.TextField(str);
                            };
                        }
                        break;
                }
                if (view != null)
                {

                    for (var i = 0; i < list.Value.Data.Count; i++)
                    {

                        list.Value.Data[i] = view(list.Value.Data[i]);
                    }
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
            defineCache = EditorGUILayout.Popup(defineCache, _typeList.ConvertAll(_=>_.ToString()).ToArray());
            //GUILayout.EndHorizontal();
            if (GUILayout.Button("+"))
            {
                AddData(labelCache, _typeList[defineCache]);
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

        private void AddLoadButton()
        {
            if (GUILayout.Button("LOAD", GUILayout.Width(100)))
            {
                LoadFromJson();
            }
        }
        private void AddSaveToJsonButton()
        {
            //var str = EditorUtil.DragAndDropRect();
            if (GUILayout.Button("SAVE AS", GUILayout.Width(100)))
            {
                SaveToJson();
            }

        }

        private void LoadFromJson()
        {
            var json = TableToJson.LoadFromJson();
            if (json == null || json.Count == 0)
            {
                return;
                
            }
            Data.Clear();
            
            foreach (var dictionary in json)
            {
                foreach (var keyValuePair in dictionary)
                {
                    if (!Data.ContainsKey(keyValuePair.Key))
                    {
                        Data.Add(keyValuePair.Key,(new List<string>(),typeof(string),50));
                    }
                    Data[keyValuePair.Key].Data.Add(keyValuePair.Value.ToString());
                }
            }
        }

        private void LoadFromClass(int index)
        {
            Debug.Log("Load");
            var t = _classes[index];
            FieldInfo[] fields 
                = t.GetFields();

            Data.Clear();
            foreach (var fieldInfo in fields)
            {
                if (!Data.ContainsKey(fieldInfo.Name))
                {
                    Data.Add(fieldInfo.Name, (new List<string>(), fieldInfo.FieldType, 50));
                }
                Debug.Log(fieldInfo.Name);
                Debug.Log(fieldInfo.GetType());
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
           
            
            TableToJson.MakeJson(list);
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
        static string GetTemplatePath()
        {
            return "";
        }
    }

    class TableToJson
    {
        public static void MakeJson( List<Dictionary<string, string>> list)
        {
            var json = MiniJSON.Json.Serialize(list);
            Debug.Log(json);
            EditorUtil.SaveJsonDialog(json);

        }

        public static List<Dictionary<string, object>> LoadFromJson()
        {
            var d = EditorUtil.LoadDialog("json");
            Debug.Log(d);
            if (string.IsNullOrEmpty(d))
            {
                return new List<Dictionary<string, object>>();
            }

            var list = new List<Dictionary<string, object>>();
            var json = (IList)MiniJSON.Json.Deserialize(d);
            foreach (var o in json)
            {
                var dic = (IDictionary)o;
                var ret = new Dictionary<string, object>();
                foreach (DictionaryEntry o1 in dic)
                {
                    ret.Add(o1.Key.ToString(),o1.Value);
                }
                list.Add(ret);
            }
            Debug.Log(list);
            return list;
        }
        public static void MakeDefJson()
        {

        }
    }

}