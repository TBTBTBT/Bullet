using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Toast;
using UnityEngine;

public class DramaManager : SingletonMonoBehaviour<DramaManager>
{

    public IEnumerator Play(DramaType type)
    {
        DramaModel model = null;
        try
        {
            var data = MasterdataManager.Records<MstDramaRecord>().FirstOrDefault(mst => mst.Type == type);
            model = MasterToModel(data);
        }
        catch (Exception e)
        {
            Debug.Log("<color=red>NoData</color>");
            yield break;
        }
        
    }

    private DramaModel MasterToModel(MstDramaRecord record)
    {
        return null;
    }
}
