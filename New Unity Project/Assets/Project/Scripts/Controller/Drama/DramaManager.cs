using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Toast;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public enum DramaType
{
    None,
    [ResourcePath("Drama_00_GameStart")]
    GameStartDrama,
    [ResourcePath("Drama_01_WeaponGet")]
    WeaponGet,
    Carry,
    Theft,
    Peddler,


}
public class DramaManager : SingletonMonoBehaviour<DramaManager>
{
    private string DramaResourceRoot = "Drama/";
    DramaView _viewer;
    protected override void Awake()
    {
        base.Awake();

       
    }

    public IEnumerator Play(DramaType type)
    {
        _viewer = PrefabManager.Instance.InstantiateOn(PrefabModel.Path.DramaView).GetComponent<DramaView>();
        Debug.Log("[ DramaManager ] Play");
        var nowType = type;
        while (type != DramaType.None)
        {
            DramaModel model = null;
            try
            {
                //var data = MasterdataManager.Records<MstDramaRecord>().FirstOrDefault(mst => mst.Type == type);
                var data = Resources.Load<DramaDataScriptableObject>(DramaResourceRoot + type.GetAttribute<ResourcePath>().Path);
                model = DataToModel(data);
                //MasterToModel(data);
            }
            catch (Exception e)
            {
                Debug.Log("<color=red>NoData</color>");
                yield break;
            }
            foreach (var frame in model.Frame)
            {
                var count = 0;

                foreach (var sprite in frame.BackSprite)
                {
                    _viewer.SetBG(sprite, count);
                    count++;
                }
                count = 0;
                foreach (var sprite in frame.LeftChara)
                {
                    _viewer.SetLeft(sprite, count);
                    count++;
                }
                count = 0;
                foreach (var sprite in frame.RightChara)
                {
                    _viewer.SetRight(sprite, count);
                    count++;
                }
                yield return SetText(frame.Text);
                yield return null;
            }
            type = DramaType.None;
            if (model.Next != null && model.Next.Length > 0)
            {
                var select = 0;
                if (model.Next.Length > 1)
                {
                    var names = new List<string>();
                    foreach (var n in model.Next)
                    {
                        names.Add(n.Name);
                    }
                    yield return InputManager.Instance.WaitForSelect(
                        PlayerManager.Instance.CurrentPlayerModel,
                        names.ToArray(),
                        num => select = num);
                }
                type = model.Next[select].Type;
            }
            if(model.Rewards != null && model.Rewards.Length > 0)
            {
                yield return GetReward(model);
            }
            yield return GetReward(model);
        }
        Destroy(_viewer.gameObject);
    }
    private IEnumerator GetReward(DramaModel model)
    {
        if (model.Rewards == null)
        {
            yield break;
        }
        foreach (var item in model.Rewards)
        {

        }


        yield return null;
    }
    IEnumerator SetText(string text)
    {
        var msg = "";
        foreach (char c in text)
        {
            msg += c;
            _viewer.SetText(msg);
            yield return null;
            if (Input.GetMouseButtonDown(0)){
                break;
            }
        }
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
    }
    private DramaModel DataToModel(DramaDataScriptableObject record)
    {
        return record.Model;
    }
}
