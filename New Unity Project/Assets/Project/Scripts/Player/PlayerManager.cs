using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using Toast;
public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    private List<PlayerBase> _players = new List<PlayerBase>();
    private int _index = 0;
    public PlayerBase CurrentPlayer => _players.Count >= _index && _index >= 0 ? _players[_index] : null;

    public void AddPlayer(string id, PlayerType type)
    {
        Debug.Log($"[PlayerManager]AddPlayer({id},{type})");
        switch (type)
        {
            case PlayerType.Local:
                _players.Add(new LocalPlayer().Init(id));
                break;
            case PlayerType.Network:
                _players.Add(new NetworkPlayer().Init(id));
                break;
            case PlayerType.Com:
                _players.Add(new ComPlayer().Init(id));
                break;
        }


    }
    public void UpdatePlayer()
    {
        _players.ForEach(_ => _.Update());
    }

}
