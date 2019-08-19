using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Toast;
public class MapController
{
    private MapSercher _mapSercher;
    private MapModel _mapModel;
    public enum PlaceType
    {
        None = 0,
        Normal,
        Shop,
        Start,
        Village,

    }

    public void Init()
    {
        Debug.Log("[MapController]Init");
        _mapSercher = new MapSercher();
        _mapModel = new MapModel();
        SetDebugMap();
    }
    public MapSercher MapSercher => _mapSercher;
    public MapModel MapModel => _mapModel;
    #region Debug
    public void SetDebugMap()
    {
        _mapModel = SampleMap();
    }
    private MapModel SampleMap()
    {
        var map = new int[100,100];
        for(int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                map[x, y] = 1;
            }
        }
        return new MapModel()
        {
            Data = map,

        };
    }
    #endregion
}


