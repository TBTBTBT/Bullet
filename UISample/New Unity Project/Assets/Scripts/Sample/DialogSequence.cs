using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Toast;
using Toast.Rg;
using UnityEngine.Events;
using UnityEngine;
public class DialogSequence : SequenceBase<DialogSequence.State>
{
    public enum State
    {
        Start,
        Execute,
        End
    }


    protected override State StartState() => State.Start;
    protected override State EndState() => State.End;
    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();
    public DialogSequence(Action<DialogSequence> init = null) => init?.Invoke(this);
    protected override void InitStatemachine() => _statemachine.Init(this);
    // Start is called before the first frame update
    IEnumerator Start()
    {
        _statemachine.Next(State.Execute);
        yield return null;
    }

    IEnumerator Execute()
    {
        //ステートマシン上で動作するダイアログです。
        //
        yield return DialogSingleton.OpenModal(new Dialog.InputData<DialogMessageAndButton>()
        {
            SetContentsPrefab = PrefabManager.GetPrefab(PrefabModel.Path.DialogNormal).GetComponent<DialogMessageAndButton>(),
            SetContentsInit = dialog =>
            {
                dialog.Init(new List<MessageSet>()
                {
                    new MessageSet()
                    {
                        Name = "Test",
                        Message = "非処理同期に対応したダイアログです。\nダイアログが閉じるまで処理を待ちます。",
                        Button = new Dictionary<string, Func<bool>>()
                        {
                            {
                                "Next", () =>
                                {
                                    dialog.DoNext();
                                    return false;
                                }
                            }
                        }
                    },
                    new MessageSet()
                    {
                        Name = "Test",
                        Message = "ボタン数やメッセージ速度など細かく設定できます。",
                        Button = new Dictionary<string, Func<bool>>()
                        {
                            {
                                "Next", () =>
                                {
                                    dialog.DoNext();
                                    return false;
                                }
                            },
                            {
                                "Cancel", () => true
                            },
                        }
                    }
                    ,
                    new MessageSet()
                    {
                        Name = "Test",
                        Message = "カスタマイズ次第では画像を載せたり、通信を待ったりできます。",
                        Button = new Dictionary<string, Func<bool>>()
                        {
                            {
                                "Ok", () =>
                                {
                                    dialog.DoNext();
                                    return false;
                                }
                            },
                        },
                        AsyncAction = NestTest()
                    }

                });
            }
        });
        yield return null;
        _statemachine.Next(State.End);
    }

    IEnumerator NestTest()
    {
        yield return DialogSingleton.OpenModal(new Dialog.InputData<DialogMessageAndButton>()
        {
            SetContentsPrefab = PrefabManager.GetPrefab(PrefabModel.Path.DialogNormal)
                .GetComponent<DialogMessageAndButton>(),
            SetContentsInit = dialog =>
            {
                dialog.Init(new List<MessageSet>()
                {
                    new MessageSet()
                    {
                        Name = "Test",
                        Message = "ネストで開くことも可能です。キャンセルするまで無限に開きます。",
                        Button = new Dictionary<string, Func<bool>>()
                        {
                            {
                                "Next", () =>
                                {
                                    dialog.DoNext();
                                    return false;
                                }
                                
                            },
                            {
                                "Cancel", () => true
                            },
                        },
                    },
                    new MessageSet()
                    {
                        Name = "Test",
                        Message = "",
                     
                        AsyncAction = NestTest(),
                        Action = DialogSingleton.Close
                    }
                });
            }
        });
    }
}
