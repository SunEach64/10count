using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCount : MonoBehaviour
{
    [Header("エネミー名")] public string[] eName;
    //[0]：通常ゾンビ,Zombie
    //[1]：ボスゾンビ,BossZombie
    //[2]：赤ゾンビ,Redrum
    //[？]：ゴースト(予定)
    [HideInInspector] public int[] eCount; //撃破カウント
    [HideInInspector] public string[] result; //リザルト文


    public void CountPuls(int n)
    {
        eCount[n] += 1; //対応番号の敵撃破カウントを加算
    }

    
}
