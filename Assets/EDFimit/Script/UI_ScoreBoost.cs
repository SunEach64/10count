using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//�c�莞�Ԃɉ������X�R�A�������̕\��
//G�}�l�[�W���[�̃X�R�A�u�[�X�g�ϐ������̂܂ܗ��p�ł���
//�^�C�}�[�Ɏc�莞��10�b�ȏ��Ԃ𔻒肳���AG�}�l�[�W���[�o�R�ł��̃X�N���v�g���A�^�b�`���ꂽ�I�u�W�F�N�g���A�N�e�B�u�ɂ���

public class UI_ScoreBoost : MonoBehaviour
{
    private TMP_Text bt;

    void Start()
    {
        bt = GetComponent<TMP_Text>();
    }

    void Update()
    {
        bt.text = "ScoreBoost " + (GManager.instance.scoreBoost * 100).ToString("f0") + "%";
    }
}
