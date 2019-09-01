using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameSystemController
{
    private MapController _mapController;
    private MapUIManager _mapUi;
    public int Turn { get; private set; }
    private int CurrentPlayerIndex => PlayerManager.Instance.CurrentPlayerIndex;


    public void Init()
    {
        Turn = 0;
        _mapController = new MapController();
        _mapController.Init();
        PrefabManager.Instance.InstantiateOn(PrefabModel.Path.MapUI);
        MapUIManager.Instance.Set(_mapController.MapView);
        PlayerManager.Instance.InitPlayerView();

    }
    public IEnumerator CalcMovable(Vector2Int start,int num)
    {
        yield return _mapController.MapSercher.Search(_mapController.MapModel.Data, start, num);
        Debug.Log($"移動候補 {_mapController.MapSercher.Result.Count} マス");
    }
    //CalcMovableした後
    public bool CheckMovable(Vector2Int pos)
    {
        var search = _mapController.MapSercher.Result.Where(p => p.pos == pos).ToArray();
        return search.Length > 0;
    }
    public void NextTurn()
    {
        PlayerManager.Instance.NextPlayer();
        if (PlayerManager.Instance.IsNewTurn())
        {
            Turn++;
        }
    }
    public void SaveGame()
    {

    }
}
