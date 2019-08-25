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

    }
    public static Dictionary<ObjectType, (TilePath,TileLayer)> TypeToAsset = new Dictionary<ObjectType, (TilePath, TileLayer)>()
    {
        {ObjectType.WeedField,(TilePath.WeedFloor,TileLayer.Field)},
    };
    [SerializeField]
    private int[,] data;

    public int[,] Data { get => data; set => data = value; }
    public ObjectType[,] Tile { get; set; }


}