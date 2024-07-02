using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S�Ă̓G�ɋ��ʂŃA�^�b�`����A���̂����p�X�N���v�g
//�X�N���v�^�u���I�u�W�F�N�g��������o�R���Ĉ���

public class EnemyParameter : MonoBehaviour
{
    [HideInInspector] public float hp; //����HP
    [HideInInspector] public float kbForce; //�o���b�g����擾
    [HideInInspector] public float kbTime; //�o���b�g����擾
    [HideInInspector] public Vector3 kbDir; // //�o���b�g����擾
    [HideInInspector] public bool _death; //���S����
    [HideInInspector] public bool _knockBack; //�m�b�N�o�b�N����

    [SerializeField] S_EnemyData eData; //�X�N���v�^�u���I�u�W�F�N�g
    

    void Start()
    {
        hp = eData.hp; //HP��ݒ�
    }

    void Update()
    {
        
    }

    /// <summary>
    /// �_���[�W����
    /// </summary>
    /// <param name="dam"></param>
    public void DamageCount(float dam, float kbF, float kbT, Vector3 kbD)
    {
        //�󂯂��U��(�o���b�g��)����_���[�W��m�b�N�o�b�N�����󂯎��AHP���������ƃX�R�A�������s���A�{�̃X�N���v�g�փm�b�N�o�b�N���𑗂�

        //HP��������
        hp -= dam;

        //���S���ǉ�����
        if (hp <= 0)
        {
            _death = true; //�G�l�~�[�{�̑��X�N���v�g���ǂݎ�邽�߂̎��S����
            GManager.instance.ScoreCount(eData.score); //�X�R�A���Z�Ăяo��
            GManager.instance.ec.CountPuls(eData.num);//���j�J�E���g���Z�Ăяo��
        }

        //�m�b�N�o�b�N����(�����ɂ�苓����ς��邽�߈ꉞ_death�̔������ɓ����)
        kbForce = kbF;
        kbTime = kbT;
        kbDir = kbD;
        _knockBack = true;

        //���{�X�̓m�b�N�o�b�N�Ȃ�(�{�̃X�N���v�g���Ƀm�b�N�o�b�N���\�b�h�����Ȃ���Ώ���ɖ��������)
    }
}
