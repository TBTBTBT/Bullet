using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapModel
{
    [SerializeField]
    private int[,] data;

    public int[,] Data { get => data; set => data = value; }



}