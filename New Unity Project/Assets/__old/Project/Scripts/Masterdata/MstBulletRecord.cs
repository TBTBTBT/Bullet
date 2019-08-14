using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
[MasterPath("/Master/mst_bullet.json")]
public class MstBulletRecord : IMasterRecord
{
    public int Id { get => id; }

    public int id;
    public float velH;//進行方向に対して水平の加速度
    public float velV;//進行方向に対して垂直の加速度
    public float accH;
    public float accV;
    public int attack;
    public int priority;
    public int lifeTimeFrame;
    public int calcId;
    public float radius;
    public float radiusVel;
    public float radiusAcc;
    public bool isAimTarget;


}
