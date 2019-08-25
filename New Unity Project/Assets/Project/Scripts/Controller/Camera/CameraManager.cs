using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toast;
public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    public Camera GameCamera => Camera.main;
    public Camera UICamera => Camera.main;
    public void SetCamera()
    {

    }
}
