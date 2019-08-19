using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toast;
using WebSocketSharp;
public class WSClient : SingletonMonoBehaviour<WSClient>
{
    private string _addr = "localhost";
   
    WebSocket _ws;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void Connect(string addr)
    {
        _ws = new WebSocket($"ws://{addr}:{WSServer.Port}");
        _ws.OnOpen += (sender, e) =>
        {
            Debug.Log("[Client]WebSocket Open");
        };
        _ws.OnMessage += (sender, e) =>
        {
             Debug.Log("[Client]WebSocket Message Type: " + e.GetType() + ", Data: " + e.Data);
        };

        _ws.OnError += (sender, e) =>
        {
            Debug.Log("[Client]WebSocket Error Message: " + e.Message);
        };

        _ws.OnClose += (sender, e) =>
        {
            Debug.Log("[Client]WebSocket Close");
        };
        _ws.Connect();
    }
    public void Send(string data)
    {
        _ws.Send(data);
    }
    void OnDestroy()
    {
        _ws?.Close();
        _ws = null;
    }
#if UNITY_EDITOR
    private string _debug;
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.Label($"wsconnect : ");
        _addr = GUILayout.TextField(_addr);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        _debug = GUILayout.TextField(_debug);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Connect"))
        {
            Connect(_addr);
        }
        if (GUILayout.Button("SendTest"))
        {
            Send(_debug);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
#endif
}
