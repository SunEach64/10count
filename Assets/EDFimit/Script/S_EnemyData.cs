using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData")]
public class S_EnemyData : ScriptableObject
{
    [Header("番号")] public int num;
    [Header("名前")] public string eName;
    [Header("HP")] public float hp;
    [Header("攻撃力")] public float atk;
    [Header("スコア")] public float score;
}
