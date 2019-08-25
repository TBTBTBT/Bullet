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
    private MapView _mapView;


    public void Init()
    {
        Debug.Log("[MapController]Init");
        _mapSercher = new MapSercher();
        _mapModel = new MapModel();
        _mapView = PrefabManager.Instance.InstantiateOn(PrefabModel.Path.MapGrid).GetComponent<MapView>();
        _mapView.transform.position = Vector3.zero;
        SetDebugMap();
        _mapView.Init(MapModel);
    }

    public MapSercher MapSercher => _mapSercher;
    public MapModel MapModel => _mapModel;
    public MapView MapView => _mapView;
    #region Debug
    public void SetDebugMap()
    {
        _mapModel = SampleMap();
    }
    private MapModel SampleMap()
    {
        var map = new int[100,100];
        var tile = new ObjectType[100, 100];
        for(int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                map[x, y] = 1;
                tile[x, y] = ObjectType.WeedField;
            }
        }
        return new MapModel()
        {
            Data = map,
            Tile = tile
        };
    }
    #endregion
}


