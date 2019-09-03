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
        var mapWidth = 30;
        var map = new StationModel[mapWidth * mapWidth];
        var tile = new ObjectType[100, 100];
        for(int x = 0; x < tile.GetLength(0); x++)
        {
            for (int y = 0; y < tile.GetLength(1); y++)
            {
                tile[x, y] = ObjectType.WeedField;
            }
        }

        for (int x = 0; x < map.Length; x++)
        {
            map[x] = new StationModel(x);
            var relation = new List<int>() {x - 1, x - map.Length / mapWidth, x + 1, x + map.Length / mapWidth};
            if (x % mapWidth == 0)
            {
                relation.RemoveAll(r => r == x - 1);
            }
            if (x % mapWidth == mapWidth - 1)
            {
                relation.RemoveAll(r => r == x + 1);
            }
            
            map[x].Pos = new Vector2Int(x % mapWidth,(int)(x / mapWidth));
            map[x].Init(relation,map.Length);
        }
        return new MapModel()
        {
            Data = map,
            Tile = tile
        };
    }
    #endregion
}


