using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCount : MonoBehaviour
{
    [Header("�G�l�~�[��")] public string[] eName;
    //[0]�F�ʏ�]���r,Zombie
    //[1]�F�{�X�]���r,BossZombie
    //[2]�F�ԃ]���r,Redrum
    //[�H]�F�S�[�X�g(�\��)
    [HideInInspector] public int[] eCount; //���j�J�E���g
    [HideInInspector] public string[] result; //���U���g��


    public void CountPuls(int n)
    {
        eCount[n] += 1; //�Ή��ԍ��̓G���j�J�E���g�����Z
    }

    
}
