using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlaceType
{
    None = 0,
    Normal,
    Shop,
    Start,
    Village,

}
public enum TileLayer
{
    Field,
    Road,
    Object,

}

public enum ObjectType
{
    WeedField,
    RockField,
    Fire,
    Road,
    Rock,
    Bush,
    Tree,
}
[Serializable]
public class MapModel
{
    public const string TilePathRoot = "Map/Tiles/";
    public enum TilePath
    {
        [ResourcePath(TilePathRoot + "Floor/Tile_Floor_1001")]
        WeedFloor,
        [ResourcePath(TilePathRoot + "Floor/Tile_Floor_1003")]
        RockFloor,
        [ResourcePath(TilePathRoot + "Floor/Tile_Floor_1001")]
        SelectTile,
    }
    public static Dictionary<ObjectType, (TilePath,TileLayer)> TypeToAsset = new Dictionary<ObjectType, (TilePath, TileLayer)>()
    {
        {ObjectType.WeedField,(TilePath.WeedFloor,TileLayer.Field)},
    };

    private StationModel[] _data;
    public StationModel[] Data { get => _data; set => _data = value; }
    public ObjectType[,] Tile { get; set; }


}
/// <summary>
/// すごろくのマス
/// </summary>
public class StationModel
{
    //public int Id;
    public PlayerEventMapType Type;
    public Vector2Int Pos;
    public int[] Relation;
    public int Id;

    public StationModel(int id)
    {
        Id = id;
    }
    public void Init(List<int> relation,int len)
    {
       relation.RemoveAll(r => r < 0 || r >= len);
       Relation = relation.ToArray();
    }
}
