using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BattleSequence : NestSequence<BattleSequence.State>
{
    public enum State
    {
        Start,
        WaitInput,
        Battle,
        Result,
        FadeOut,
        End


    }
    protected override BattleSequence.State StartState() => BattleSequence.State.Start;
    protected override BattleSequence.State EndState() => BattleSequence.State.End;

    public UnityEvent OnEnd = new UnityEvent();

    public GameSystemController Game;
    private BattleController BattleController;
    private int _diceNum = 0;
    public BattleSequence(System.Action<BattleSequence> init)
    {
        init?.Invoke(this);
    }
    IEnumerator Start()
    {
        Debug.Log("[DiceSeq] Start");
        if (PlayerManager.Instance.CurrentPlayerModel.CurrentBattle == null)
        {
            BattleController = new BattleController();
            BattleController.SetPlayer(CreateBattleChara(PlayerManager.Instance.CurrentPlayerModel));
        }
        else
        {
            BattleController = PlayerManager.Instance.CurrentPlayerModel.CurrentBattle;
        }

        var currentChara = BattleController.Chara;


        var otherChara = SearchAndCreateBattleCharaModels(
            PlayerManager.Instance.CurrentPlayerModel.Id ,
            PlayerManager.Instance.CurrentPlayerModel.Status.MapPos);//同じマスのぷれいやーけんさく
        //すでに戦闘中の敵がいれば選択肢に追加
        otherChara.AddRange(currentChara.Where(c=>c.PlayerId != PlayerManager.Instance.CurrentPlayerModel.Id).ToList());
       
        BattleCharaModel selected = null;
        if (otherChara.Count > 0)
        {
            var index = 0;
            if (otherChara.Count > 1)
            { //選ばせる。
                yield return InputManager.Instance.WaitForSelect(
                    PlayerManager.Instance.CurrentPlayerModel,
                    otherChara.Select(c=>c.Name).ToArray(),
                    num => { index = num; });
                
            }

            selected = otherChara[index];
            BattleController.Clear();
            BattleController.SetPlayer(CreateBattleChara(PlayerManager.Instance.CurrentPlayerModel));
            BattleController.SetPlayer(selected);
        }
        else
        {
            //新しいエネミー
            //todo ますたーからひっぱる
            BattleController.SetPlayer(new BattleCharaModel()
            {
                additionalStatus = new StatusModel(),
                status =  new StatusModel(),
                isPlayer = false,
                Name =  "dummy"
            });

        }

        

        _statemachine.Next(BattleSequence.State.WaitInput);
        yield return null;
    }
    IEnumerator WaitInput()
    {
        var indexies = new List<int>(){-1,-1};
        var count = 0;
        foreach (var battleCharaModel in BattleController.Chara)
        {
            if (battleCharaModel.isPlayer)
            {
                StartCoroutine(InputManager.Instance.WaitForSelect(
                    PlayerManager.Instance.FindPlayerModel(battleCharaModel.PlayerId),
                    PlayerManager.Instance.FindPlayerModel(battleCharaModel.PlayerId)., //技データ
                    num => { indexies[count] = num; }));
            }

            count++;
        }

        Debug.Log("[DiceSeq] WaitInput");
        _statemachine.Next(BattleSequence.State.Battle);
        yield return null;
    }
    IEnumerator Battle()
    {
 
        _statemachine.Next(BattleSequence.State.Result);
        yield return null;
    }
    IEnumerator Result()
    {

        _statemachine.Next(BattleSequence.State.FadeOut);
        yield return null;
    }
    IEnumerator FadeOut()
    {

        _statemachine.Next(BattleSequence.State.End);
        yield return null;
    }
    protected override void InitStatemachine()
    {
        _statemachine.Init(this);
    }

    private List<string> SearchSameStationPlayer(string me , int pos)
    {
        return PlayerManager.Instance.Players.Where(player => (player.Status.MapPos == pos && player.Id != me)).Select(player => player.Id).ToList();
    }

    private List<BattleCharaModel> SearchAndCreateBattleCharaModels(string me, int pos)
    {
        return SearchSameStationPlayer(me, pos)
            .ConvertAll(s => CreateBattleChara(PlayerManager.Instance.FindPlayerModel(s)));
    }
    private BattleCharaModel CreateBattleChara(PlayerModel player)
    {
        return new BattleCharaModel()
        {
            additionalStatus = new StatusModel(),
            status = player.Status,
            Name = player.Status.Name,
            PlayerId = player.Id,
            isPlayer = true
        };
    }
}
