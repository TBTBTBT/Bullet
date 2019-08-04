using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BulletPhysics Physics;
    public int MaxTurn = 720;
    public bool Finish => Physics?.FrameCount >= MaxTurn;
    public void Init()
    {
        Physics = new BulletPhysics();
    }
    public void AddCharacter()
    {
        Physics.Initialize();
        //todo charaAdd
    }
    public void Simurate()
    {
        Physics.Simulate();
    }
}
