using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PacketType
{
    [Packet(null, null)]
    None,
    [Packet(typeof(AppEntryRequest), typeof(AppEntryResponce))]
    Entry,

}
/// <summary>
/// 参加リクエスト
/// </summary>
[Serializable]
public class AppEntryRequest
{

}
[Serializable]
public class AppEntryResponce
{

}