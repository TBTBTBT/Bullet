using System;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using Toast;
using Open.Nat;
public class WSServer : SingletonMonoBehaviour<WSServer>
{ 
    public class StringEvent :UnityEvent<string>{ }
    public enum ServerState
    {
        None,
        Boot,
        Work,
        Error
    }
    public static readonly int Port = 43586; 
    private static ServerState _state = ServerState.None;
    private static bool UseNATTraversal = false;
    public static ServerState State => _state;
    WebSocketServer server;

    private Dictionary<Type, StringEvent> _onMessageEvents = new Dictionary<Type, StringEvent>();
    public Dictionary<Type, StringEvent> OnMessageEvents => _onMessageEvents;
    public void OnReceiveRequest<T>(Action<T> cb) where T : PacketModelBase
    {
        //var packetType = typeof(T).GetPacketType();
        UnityAction<string> addEvent = str =>
        {
            var data = str.ToData<T>();
            cb(data);
        };
        if (_onMessageEvents.ContainsKey(typeof(T)))
        {
            _onMessageEvents[typeof(T)].AddListener(addEvent);
            return;
        }
        _onMessageEvents.Add(typeof(T), new StringEvent());
        _onMessageEvents[typeof(T)].AddListener(addEvent);
    }
    public void RemoveEvent<T>() where T : PacketModelBase
    {
        _onMessageEvents[typeof(T)].RemoveAllListeners();
        _onMessageEvents.Remove(typeof(T));
    }
    public void ResetEvent()
    {
        _onMessageEvents.Clear();
    }

    async Task NATDiscover()
    {
        Debug.Log("[OpenNAT] NATDiscover");
        var discoverer = new NatDiscoverer();
        var cts = new CancellationTokenSource(10000);

        var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);
        var ip = await device.GetExternalIPAsync();
        Debug.Log($"The external IP Address is: {ip.Address.ToString()} ");
        await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, Port, Port, "D server"));
        Debug.Log("[OpenNAT] Finish");
    }
    IEnumerator BootServer()
    {
        _state = ServerState.Boot;
        //NAT越え
        if (UseNATTraversal)
        {
            var discover = Task.Run(NATDiscover);
            while (!discover.IsCompleted)
            {
                yield return null;
            }
        }
        server = new WebSocketServer($"ws://{ScanIPAddr.Get()[0]}:{ Port }");
        server.AddWebSocketService<Echo>("/");
        server.Start();

        _state = ServerState.Work;
    }

    void Start()
    {
        StartCoroutine(BootServer());
       

    }

    void OnDestroy()
    {
        server?.Stop();
        server = null;
    }
#if UNITY_EDITOR
    private void OnGUI()
    {
        if (State == ServerState.Work)
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.Label($"wsserver : {server.Address} ");
            GUILayout.Label($"port : {server.Port} ");
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
#endif
}

public class Echo : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        Debug.Log($"[Server] Message:{e.Data}");
        var packet = e.Data.ParseToProtocol();
        var type = packet.PacketType.GetAttribute<Packet>().Request;
        if (WSServer.Instance.OnMessageEvents.ContainsKey(type))
        {
            WSServer.Instance.OnMessageEvents[type]?.Invoke(e.Data);
        }
        switch (packet.EchoType)
        {
            
            case EchoType.BroadCast:
                Sessions.Broadcast(e.Data);
                break;
            case EchoType.Only:
                Sessions.SendTo(e.Data, packet.SendToId);
                break;
            case EchoType.Self:
            default:
                Sessions.SendTo(e.Data, ID);
                break;
        }
        
      
    }
}
/*
     IEnumerator UPnPSearchDevice()
    {
        IPEndPoint LocalEndPoint = new IPEndPoint(IPAddress.Any, 0);
        IPEndPoint MulticastEndPoint = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1900);

        Socket UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        UdpSocket.Bind(LocalEndPoint);


        Debug.Log("UDP-Socket setup done...");
        Debug.Log($"{MulticastEndPoint.Address} , {MulticastEndPoint.Port}");
        Debug.Log($"{LocalEndPoint.Address} , {LocalEndPoint.Port}");
        string SearchString = "M-SEARCH * HTTP/1.1\r\nHOST:239.255.255.250:1900\r\nMAN:\"ssdp:discover\"\r\nST:ssdp:all\r\nMX:3\r\n\r\n";
        Debug.Log(UdpSocket.Available);
        var byteNum = UdpSocket.SendTo(Encoding.UTF8.GetBytes(SearchString), SocketFlags.None, MulticastEndPoint);

        Debug.Log("M-Search sent...\r\n");

        byte[] ReceiveBuffer = new byte[64000];

        int ReceivedBytes = 0;
        Debug.Log(UdpSocket.LingerState);
        while (true)
        {
            if (UdpSocket.Available > 0)
            {
                ReceivedBytes = UdpSocket.Receive(ReceiveBuffer, SocketFlags.None);

                if (ReceivedBytes > 0)
                {
                    Debug.Log(Encoding.UTF8.GetString(ReceiveBuffer, 0, ReceivedBytes));
                    yield break;
                }
                Debug.Log(ReceivedBytes);
            }
            yield return null;
        }
    }
    IEnumerator BootServer()
    {
        _state = ServerState.Boot;
        yield return UPnPSearchDevice();
        server = new WebSocketServer($"ws://{ScanIPAddr.Get()[0]}:3000");

        server.AddWebSocketService<Echo>("/");
        server.Start();

        _state = ServerState.Work;
    }
    */
