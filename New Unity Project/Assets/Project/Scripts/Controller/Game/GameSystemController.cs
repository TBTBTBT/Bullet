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
        foreach (var p in PlayerManager.Instance.Players)
        {
            PlayerManager.Instance.InitPlayerView(p.Id, GetMapPos(p.Status.MapPos));
        }
        

    }

    public int GetMapIndex(Vector2Int pos)
    {
        var list = _mapController.MapModel.Data.Where(m => m.Pos == pos).ToList();
        if (list.Count > 0)
        {
            return list[0].Id;
        }

        return -1;
    }
    public Vector2 GetMapPos(int pos)
    {
        return MapUIManager.Instance.TileToWorldPos(_mapController.MapModel.Data[pos].Pos);
    }
    public void SetMapCanMoveView(bool active)
    {
        if (active)
        {
            var stations = _mapController.MapSercher.ResultToStation(_mapController.MapModel.Data);
            _mapController.MapView.SetSelectTile(stations.ToArray());
        }
        else
        {
            _mapController.MapView.ClearSelectTile();
        }
    }

    public IEnumerator CalcMovable(int start,int num)
    {
        yield return _mapController.MapSercher.Search(_mapController.MapModel.Data, start, num);
       
        
        Debug.Log($"移動候補 {_mapController.MapSercher.Result.Count} マス");
    }
    //CalcMovableした後
    public bool CheckMovable(int pos)
    {
        var search = _mapController.MapSercher.Result.Where(p => p == pos).ToArray();
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
    public void FocusPlayer()
    {
        CameraManager.Instance.GameCamera.transform.position =
            new Vector3(0, 0, -10) + (Vector3)GetMapPos(PlayerManager.Instance.CurrentPlayerModel.Status.MapPos);

    }
    public void SaveGame()
    {

    }
}
