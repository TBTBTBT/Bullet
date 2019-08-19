using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using Toast;
public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    private List<PlayerModelBase> _players = new List<PlayerModelBase>();
    private int _index = 0;
    public PlayerModelBase CurrentPlayerModel => _players.Count >= _index && _index >= 0 ? _players[_index] : null;

    public void AddPlayer(string id, PlayerType type)
    {
        Debug.Log($"[PlayerManager]AddPlayer({id},{type})");
        switch (type)
        {
            case PlayerType.Local:
                _players.Add(new LocalPlayerModel().Init(id));
                break;
            case PlayerType.Network:
                _players.Add(new NetworkPlayerModel().Init(id));
                break;
            case PlayerType.Com:
                _players.Add(new ComPlayerModel().Init(id));
                break;
        }


    }
    public void UpdatePlayer()
    {
        _players.ForEach(_ => _.Update());
    }

}
