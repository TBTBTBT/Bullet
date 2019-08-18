using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[Serializable]
public enum EchoType
{
    Self,
    BroadCast,
    Only,
}
[Serializable]
public enum Responce
{
    Error,
    Ok,
    Deny,
}
public static class PacketManager
{
    public static string ToPacket<T>(T data, EchoType type = EchoType.Self,string sendTo = "" ) where T : PacketBase
    {
        var json = JsonUtility.ToJson(data);
        return JsonUtility.ToJson(new PacketProtocol()
        {
            PacketType = typeof(T).GetPacketType(),
            EchoType = type,
            Data = json,
            SendToId = sendTo
        });
    }
    public static PacketProtocol ParseToProtocol(this string packet)
    {
        return JsonUtility.FromJson<PacketProtocol>(packet);
    }
    public static T ToData<T>(this string packet)
    {

        return JsonUtility.FromJson<T>(packet.ParseToProtocol().Data);
    }
    public static object ToData(this string packet,Type type)
    {
        return JsonUtility.FromJson(packet.ParseToProtocol().Data,type);
    }
    public static object ResponceToData(this string packet, PacketType type)
    {
        var t = type.GetAttribute<Packet>();
        return JsonUtility.FromJson(packet.ParseToProtocol().Data, t.Responce);
    }
    public static object RequestToData(this string packet, PacketType type)
    {
        var t = type.GetAttribute<Packet>();
        return JsonUtility.FromJson(packet.ParseToProtocol().Data, t.Request);
    }
    public static PacketType GetPacketType(this Type packetBase)
    {
        foreach(PacketType t in Enum.GetValues(typeof(PacketType)))
        {
            var resreq = t.GetAttribute<Packet>();
            if(resreq.Request == packetBase || resreq.Responce == packetBase)
            {
                return t;
            }
        }

        return PacketType.None;
    }
}
[Serializable]
public class PacketProtocol
{

    public EchoType EchoType;
    public PacketType PacketType;
    public string Data;
    public string SendToId;
}
[Serializable]
public class PacketBase
{
    public Responce Res;
    public string Id;
}

[Serializable]
[AttributeUsage(AttributeTargets.Field, Inherited = false)]
public class Packet : Attribute
{
    public Type Request;
    public Type Responce;
    public Packet(Type request, Type responce)
    {
        Request = request;
        Responce = responce;
    }
}