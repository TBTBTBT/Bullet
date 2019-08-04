﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toast;
public class BulletPhisicsExample : MonoBehaviour
{
    public enum State
    {
        Init,
        Play,
        Calc,
        CalcOnce,
        Pause,
        ReCalc,
        AddElement,
        End
    }

    [SerializeField] private float _timeSpan = 0.1f;
    [SerializeField] private BulletViewer _viewer;
    private Statemachine<State> _stateMachine = new Statemachine<State>();
    private BulletPhysics Phisics { get; set; }
    private bool CanTransition = false;

    void Awake()
    {
        _stateMachine.Init(this);
        Phisics = new BulletPhysics();
    }

    private IEnumerator Init()
    {
        CanTransition = false;
        //yield return Phisics.Init(100,5,()=> { });
        Phisics.Reset();
        Phisics.SetTimeSpan(_timeSpan);
        
        CanTransition = true;
        yield return null;
    }
    private IEnumerator Play()
    {
       
        yield return null;
    }

    private IEnumerator Calc()
    {
        while (true)
        {
            Phisics.Simulate();
            _viewer.UpdateView(Phisics);
            yield return null;
        }
    }
    private IEnumerator CalcOnce()
    {
        //while (true)
        {
            Phisics.Simulate();
            _viewer.UpdateView(Phisics);
            yield return null;
        }
    }
    private IEnumerator Pause()
    {
        yield return null;
    }

    private IEnumerator ReCalc()
    {
        yield return null;
    }

 

    void Update()
    {
        _stateMachine.Update();
    }

    private Dictionary<string, string> _tmp = new Dictionary<string, string>();
    private string GetTmp(string key)
    {
        return _tmp.ContainsKey(key) ? _tmp[key] : "";
       
    }

    private int GetTmpInt(string key)
    {
        var tmp = _tmp.ContainsKey(key) ? _tmp[key] : "0";
        try
        {
            return int.Parse(tmp);
        }
        catch (Exception e)
        {
            return 0;
        }
    }

    private void SetTmp(string key, string value)
    {
        if (!_tmp.ContainsKey(key))
        {
            _tmp.Add(key,"");
        }

        _tmp[key] = value;
    }
    private Dictionary<State, Action<BulletPhisicsExample>> _actions = new Dictionary<State, Action<BulletPhisicsExample>>()
    {
        {State.Init, self =>
        {
            GUILayout.Label($" Loading : {self._viewer.Loading.Percent} %");
        }
        },
        {State.ReCalc, self =>
        {
            self.SetTmp("ReCalcTime",GUILayout.TextField(self.GetTmp("ReCalcTime")));
            if (GUILayout.Button("ReCalc"))
            {
                self.Phisics.RecalcAll(self.GetTmpInt("ReCalcTime"));
            }
        }
        },
        {State.AddElement, self =>
        {
            GUILayout.Label("Id :");
            self.SetTmp("AddElementId",GUILayout.TextField(self.GetTmp("AddElementId")));
            GUILayout.Label("PosX :");
            self.SetTmp("AddElementX",GUILayout.TextField(self.GetTmp("AddElementX")));
            GUILayout.Label("PosY :");
            self.SetTmp("AddElementY",GUILayout.TextField(self.GetTmp("AddElementY")));
            GUILayout.Label("Angle :");
            self.SetTmp("AddElementAng",GUILayout.TextField(self.GetTmp("AddElementAng")));
             GUILayout.Label("Frame :");
            self.SetTmp("AddElementFrame",GUILayout.TextField(self.GetTmp("AddElementFrame")));
            if (GUILayout.Button("Add"))
            {
                self.Phisics.SetElement(
                    new Vector2(
                        self.GetTmpInt("AddElementX"),
                        self.GetTmpInt("AddElementY")),
                        0,
                        self.GetTmpInt("AddElementId"),
                        self.GetTmpInt("AddElementFrame"));
            }
        }
        },
    };
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        foreach (var value in System.Enum.GetValues(typeof(State)))
        {
            if (GUILayout.Button(value.ToString()))
            {
                if (CanTransition)
                {
                    _stateMachine.Next((State)value);
                }
            }
        }
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label("FrameCount : " + Phisics.FrameCount.ToString());
        GUILayout.Label("Time       : " + Phisics.NowTime.ToString("F1"));
        GUILayout.Label("ActiveElements: " + Phisics.ActiveElements.ToString());
        GUILayout.Label("ElementsCount: " + Phisics.ElementsCount.ToString());
        //GUILayout.Label("Elements   : " + Phisics..Count.ToString("F1"));


        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        if(_actions.ContainsKey(_stateMachine.GetCurrentState()))
        _actions[_stateMachine.GetCurrentState()](this);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
}
