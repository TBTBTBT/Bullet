using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public enum PlayerEventType
//{
//    None,
//    //停止マップが...
//    //1.通常の場合
//    ComBattle,
//    PlayerBattle,
//    CreateEquipment,//ドカポンのガッツ
//                    //2.ショップの場合
//    ShopNormal,
//    ShopAttacked,//強盗が入った

//    //3.宝箱
//    ItemBox,
//    MagicBox,
//    //4.教会
//    Heal,
//    PowerUp,
//    //5.城
//    HealInStart,



//}

[Serializable]
public class PlayerEventModel
{
    public int Id;
    public PlayerEventType Type;
    public PlayerEventMapType MapType;
    //public PlayerEventInputType InputType;
    public PlayerEventNoticeType NoticeType;
    public int Priority;
    //DramaModel Drama;
    //BattleModel Battle;
}
//[Serializable]
//[AttributeUsage(AttributeTargets.Field, Inherited = false)]
//public class PlayerEvent : Attribute
//{
//    public Type Model;
//    public Event(Type model)
//    {
//        Model = model;
//    }
//}