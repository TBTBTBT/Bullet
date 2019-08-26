using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
