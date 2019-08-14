using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public enum PlaceType
    {
        None = 0,
        Normal,
        Shop,
        Start,
        Village,

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class Map
{
    [SerializeField]
    private int[,] data;

    public int[,] Data => data;


}