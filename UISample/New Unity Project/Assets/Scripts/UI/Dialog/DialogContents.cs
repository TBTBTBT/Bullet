using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toast.Rg
{

    /// <summary>
    /// DialogのContentsに継承しなければならない
    /// </summary>
    public class DialogContents : MonoBehaviour
    {
        public Dialog Dialog { get; set; }

        public virtual void UpdateInStable()
        {
        }
    }


}