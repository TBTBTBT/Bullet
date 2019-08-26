using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using Toast;
public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    private List<PlayerModel> _players = new List<PlayerModel>();
    private int _index = 0;
    public PlayerModel CurrentPlayerModel => _players.Count >= _index && _index >= 0 ? _players[_index] : null;
    public int CurrentPlayerIndex => _index;
    public void AddPlayer(string id, PlayerType type)
    {
        Debug.Log($"[PlayerManager]AddPlayer({id},{type})");
        _players.Add(new PlayerModel().Init(type, id,new[] { 1,2,3,4,5,6}));
        //todo マスターデータから


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
}
