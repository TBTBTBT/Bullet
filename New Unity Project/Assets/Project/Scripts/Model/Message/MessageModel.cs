using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//todo MasterData
public class MessageModel
{
    public static string Get(MessageRecordsEnum type)
    {
        return type.ToString();
    }
}
public enum MessageRecordsEnum
{
    Matching = 1001,
}