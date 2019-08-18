using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    
    private void OnGUI()
    {
        GUILayout.BeginVertical();
        foreach (var s in SequenceTracer.Instance.Sequences)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{s.GetType().ToString()} : {s.CurrentState}");
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }
}
