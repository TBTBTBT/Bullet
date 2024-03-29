﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapView : MonoBehaviour
{
    [SerializeField] Tilemap _back;
    [SerializeField] Tilemap _objects;
    [SerializeField] Tilemap _road;
    [SerializeField] Tilemap _select;
    TileBase _selectTile;
    public void Init(MapModel map)
    {
        SetTile(map);
    }
    public Vector2 TileToWorldPos( Vector2Int pos)
    {
        return _back.CellToWorld((Vector3Int)pos);
    }
    public Vector2Int WorldToTilePos(Vector2 pos)
    {
        return (Vector2Int)_back.WorldToCell(pos);
    }
    void SetTile(MapModel map)
    {
        var tileCache = new Dictionary<MapModel.TilePath, (TileBase,TileLayer)>();
        for(var i = 0; i < map.Tile.GetLength(0); i++)
        {
            for (var j = 0; j < map.Tile.GetLength(1); j++)
            {
                var set = MapModel.TypeToAsset[map.Tile[i, j]];
                if (!tileCache.ContainsKey(set.Item1))
                {
                    var asset = Resources.Load<TileBase>(set.Item1.GetAttribute<ResourcePath>().Path);

                    tileCache.Add(set.Item1,(asset, set.Item2));
                }
                switch (set.Item2)
                {
                    case TileLayer.Field:
                        _back.SetTile(new Vector3Int(i, j, 0), tileCache[set.Item1].Item1);
                        break;
                    case TileLayer.Road:
                        _road.SetTile(new Vector3Int(i, j, 0), tileCache[set.Item1].Item1);
                        break;
                    case TileLayer.Object:
                        _objects.SetTile(new Vector3Int(i, j, 0), tileCache[set.Item1].Item1);
                        break;
                }
                
            }
        }
    }
    public void ClearSelectTile()
    {
        _select.ClearAllTiles();
    }
    public void SetSelectTile(StationModel[] elements)
    {
        ClearSelectTile();
        if (_selectTile == null)
        {
            _selectTile = Resources.Load<TileBase>(MapModel.TilePath.SelectTile.GetAttribute<ResourcePath>().Path);
            
        }
        foreach (var e in elements)
        {
            _select.SetTile((Vector3Int)e.Pos, _selectTile);
            _select.SetColor((Vector3Int)e.Pos, Color.red);
        }
    }
}
