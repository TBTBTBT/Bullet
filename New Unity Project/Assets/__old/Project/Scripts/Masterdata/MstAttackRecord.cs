using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//角度情報や射出数
[Serializable]
[MasterPath("/Master/mst_attack.json")]
public class MstAttackRecord : IMasterRecord
{
    public int Id { get => id; }

    public int id;
    public int num;
    public int repeatNum;//繰り返し
    public float repeatDiffAngleBetwee;//繰り返し時にずらす
    public float repeatDiffAngleDirection;//繰り返し時にずらす
    public int bulletId;//MstBulletの外部キー
    public float angleBetween;//玉と玉の間の角度
    public float angleDirection;//向きに対する全体の角度
    public float startDilay;//前隙(繰り返しには含まない)
    public float dilay;//一発ごとのすき
    public float endDilay;//後隙(繰り返しには含まない)
   



}
