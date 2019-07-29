using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Toast.Masterdata.Editor
{

    public class MasterdataEditor : EditorWindow
    {
        //classes


        //enum
        public enum State
        {
            Init,
            View,
        }


        //process

        private Dictionary<State, Action<MasterdataEditor>> StateProcess =
            new Dictionary<State, Action<MasterdataEditor>>()
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
                    self => { self.Table?.View(); }
                }

            };

        State Current { get; set; } = State.Init;
        private State NextState { get; set; } = State.Init;
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
            NextState = state;
        }

        private void Update()
        {
            if (Current != NextState)
            {
                Current = NextState;
            }
        }

        private void OnGUI()
        {
            if (StateProcess.ContainsKey(Current))
            {
                StateProcess[Current]?.Invoke(this);
            }
        }

    }

}