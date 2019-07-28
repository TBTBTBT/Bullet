using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingState
{
    public float MaxStep { get; set; }
    public float CurrentStep { get; set; }
    public float Percent => (CurrentStep / MaxStep) * 100;

}
