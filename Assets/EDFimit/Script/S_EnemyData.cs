using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData")]
public class S_EnemyData : ScriptableObject
{
    [Header("�ԍ�")] public int num;
    [Header("���O")] public string eName;
    [Header("HP")] public float hp;
    [Header("�U����")] public float atk;
    [Header("�X�R�A")] public float score;
}
