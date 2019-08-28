using System;

[Serializable]
[MasterPath("/Master/mst_drama.json")]
public class MstDramaRecord : IMasterRecord
{
    public int Id { get => id; }

    public int id;


}

public enum DramaType
{
    None,
    GameStartDrama,
    
}