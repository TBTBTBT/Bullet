using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//public class WaitForPlayerInputKey : CustomYieldInstruction
//{
//    bool _input = false;
//    public WaitForPlayerInputKey(PlayerModelBase p)
//    {
//        if (p == null)
//        {
//            p = PlayerManager.Instance.CurrentPlayerModel;
//        }
//        if (p == null)
//        {
//            _input = true;
//            return;
//        }
//        p.OnInputEvent.AddListener(select => _input = true);
//    }
//    bool Waiting()
//    {
//        //debug
//        //return !(Input.GetKey(KeyCode.A) || Input.GetMouseButtonDown(0));
//        return !_input;
//    }
//    public override bool keepWaiting => Waiting();
//}

public class WaitForSecondsForStatemachine : CustomYieldInstruction
{
    float start;
    float wait;
    public WaitForSecondsForStatemachine(float sec)
    {
        wait = sec;
        start = Time.time;
    }
    bool Waiting()
    {
        return wait > Time.time - start;
    }
    public override bool keepWaiting => Waiting();
}

//public class WaitForParalellEnumerator : CustomYieldInstruction
//{
//    private List<IEnumerator> Currents;

//    public WaitForParalellEnumerator(params IEnumerator[] functions)
//    {
//        Currents = functions.ToList();
//    }
//    bool Waiting()
//    {
//        Currents.RemoveAll(_ => !_.MoveNext());
//        return Currents.Count > 0;
//    }
//    public override bool keepWaiting => Waiting();
//}


//public class WaitForPlayerSelect : CustomYieldInstruction
//{
//    bool _decide = false;
//    public WaitForPlayerSelect(PlayerModelBase p = null)
//    {
//        if(p == null)
//        {
//            p = PlayerManager.Instance.CurrentPlayerModel;
//        }
//        if(p == null)
//        {
//            _decide = true;
//            return;
//        }
//        p.OnSelected.AddListener(select => _decide = true);
//    }

//    bool Waiting()
//    {
//        return !_decide;
//    }
//    public override bool keepWaiting => Waiting();
//}
