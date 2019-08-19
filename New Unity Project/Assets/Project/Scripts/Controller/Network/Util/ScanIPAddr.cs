using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
public class ScanIPAddr
{
    public static string[] IP { get { return Get(); } }
    public static byte[][] ByteIP { get { return GetByte(); } }

    public static string[] Get()
    {
        IPAddress[] addr_arr = Dns.GetHostAddresses(Dns.GetHostName());
        List<string> list = new List<string> ();
        foreach (IPAddress address in addr_arr)
        {
            if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                list.Add(address.ToString());
            }
        }
        if (list.Count == 0) return null;
        return list.ToArray();
    }
    public static byte[][] GetByte()
    {
        IPAddress[] addr_arr = Dns.GetHostAddresses(Dns.GetHostName());
        List<byte[]> list = new List<byte[]> ();
        foreach (IPAddress address in addr_arr)
        {
            if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                list.Add(address.GetAddressBytes());
            }
        }
        if (list.Count == 0) return null;
        return list.ToArray();
    }
}