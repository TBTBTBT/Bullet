using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Toast;
using System;
public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    private List<PlayerModel> _players = new List<PlayerModel>();
    private Dictionary<string,PlayerView> _playerViews = new Dictionary<string, PlayerView>();
    private int _index = 0;
    public PlayerModel CurrentPlayerModel => _players.Count >= _index && _index >= 0 ? _players[_index] : null;
    public int CurrentPlayerIndex => _index;
    public List<PlayerModel> Players => _players;
    public void AddPlayer(string id, PlayerType type)
    {
        Debug.Log($"[PlayerManager]AddPlayer({id},{type})");
        _players.Add(new PlayerModel().Init(type, id,new[] { 1,2,3,4,5,6}));
        //todo マスターデータから
        

    }

    public PlayerModel FindPlayerModel(string id)
    {
        try
        {
            return _players.First(p=>p.Id == id);
        }
        catch (Exception e)
        {
            Debug.Log("Player NotFound");
            return null;
        }
       
    }
    //一巡したらtrue
    public void NextPlayer()
    {
        
        _index = (_index + 1) % _players.Count;
        
    }

    public bool IsNewTurn()
    {
        return _index == 0;
    }
    //view

    public void InitPlayerView(string id,Vector2 pos)
    {

        _playerViews.Add(id, PrefabManager.Instance.InstantiateOn(PrefabModel.Path.PlayerView).GetComponent<PlayerView>());
        MovePlayer(pos,id, true);
    }
    
    public void MovePlayer(Vector2 pos, string id,bool immediate = false)
    {
        try
        {
            
            _playerViews[id].transform.position = pos;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

    }
    public void MoveCurrentPlayer(Vector2 pos )
    {
        MovePlayer(pos, CurrentPlayerModel.Id);
    }
}
