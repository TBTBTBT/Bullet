using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppBoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        MasterdataManager.Instance.InitMasterdata();
    }   


}
