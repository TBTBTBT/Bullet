using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
public class MasterdataEditor : EditorWindow
{
    class TableData
    {
        public Dictionary<string, List<string>> Data = new Dictionary<string, List<string>>();
        string labelCache = "";
        public void AddData(string label)
        {
            Data.Add( label,new List<string>() );
        }
        public void View()
        {
            
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
          
            labelCache = GUILayout.TextArea(labelCache);
            
            if (GUILayout.Button("+"))
            {
                labelCache = "";
            }
            GUILayout.EndHorizontal();
            if (GUILayout.Button("+"))
            {

            }
            GUILayout.EndVertical();
        }
    }
    class TableToClass
    {

    }
    class TableToJson
    {

    }

    public enum State
    {
        Init,
        View,
    }

    private Dictionary<State, Action<MasterdataEditor>> StateProcess = new Dictionary<State, Action<MasterdataEditor>>()
    {
        {
            State.Init,
            self =>
            {
                self.Table = new TableData();
                self.Next(State.View);
              
            }
        },
        {
            State.View,
            self =>
            {
                self.Table?.View();   
            }
        }

    };
    State Current { get; set; } = State.Init;
    TableData Table { get; set; }
    [MenuItem("Toast/MasterdataEditor")]
    static void Open()
    {
        var window = CreateInstance<MasterdataEditor>();
        window.Show();
    }
    void UIDraw()
    {

    }
    void Next(State state)
    {
        Current = state;
    }
    private void OnGUI()
    {
        if (StateProcess.ContainsKey(Current))
        {
            StateProcess[Current]?.Invoke(this);
        }
    }
    
}
