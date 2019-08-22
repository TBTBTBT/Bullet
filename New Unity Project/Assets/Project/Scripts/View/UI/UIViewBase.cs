using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIViewBase : MonoBehaviour
{
    public virtual void Dispose()
    {
        Destroy(this.gameObject);
    }
}
