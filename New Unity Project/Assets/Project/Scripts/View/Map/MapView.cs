using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapView : MonoBehaviour
{
    [SerializeField] Tilemap _back;
    [SerializeField] Tilemap _objects;
    [SerializeField] Tilemap _road;
    public void Init(MapModel map)
    {
        SetTile(map);
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
}
