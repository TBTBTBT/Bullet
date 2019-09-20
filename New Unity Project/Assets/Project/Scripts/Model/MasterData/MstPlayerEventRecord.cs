
using System;

[Serializable]
[MasterPath("/Master/mst_player_event.json")]
public class MstPlayerEventRecord : IMasterRecord
{
    public int Id { get => id; }

    public int id;
    public int Priority;
    public int Odds;
    public PlayerEventMapType MapType;
    public PlayerEventNoticeType NoticeType;
    public PlayerEventType EventType;
    public DramaType DramaType;
    

}
/*
    public PlayerEventInputType InputType;
    public PlayerEventGetRewardType RewardType;
    public int DramaId;
    public int RandomTableId;
 */
public enum PlayerEventMapType
{
    None,
    Normal,
    Shop,
    Box,
    Town,
    Church,
    Castle,
}
public enum PlayerEventType
{
    [EventType(typeof(Pev00_Battle))]
    Battle,
    [EventType(typeof(Pev01_WeaponGet))]
    WeaponGet,
    Carry,
    Theft,
    Peddler,
}
public enum PlayerEventInputType
{
    None,//ドラマのみ,バトルのみなど
    Chart,//選択肢 (複数可能、木構造可能)
    Button,


}
//!とか出すか
public enum PlayerEventNoticeType
{
    None,
    Exclamation,


}
