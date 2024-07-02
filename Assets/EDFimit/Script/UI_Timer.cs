using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//�Q�[�����̐�������
//0�ɂȂ�����^�C���X�P�[���ŃQ�[�����~�߂ă��U���g�ƃ��Z�b�g�{�^���\��

public class UI_Timer : MonoBehaviour
{
    [Header("��������")] public float timeCount = 5f;

    private TMP_Text timerText; //�������Ԃ̕\��
    private bool isDead; //���S����(DeadForce��A���쓮�����Ȃ�����)
    private bool isGOver; //�Q�[���I�[�o�[�t���O
    private bool isResult; //���U���g�֑J�ڃt���O(�J�ڌ�͖��ʂȏ������~�߂�)
    private float countGOver; //�Q�[���I�[�o�[����̌o�ߎ���

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (!isGOver)
        {
            if (timeCount <= 0 || GManager.instance.pDeath) //���Ԑ؂�or�v���C���[���S�𔻒�
            {
                if(GManager.instance.pDeath)
                {
                    PlayOut();
                }
                else //������ԂŎ��Ԑ؂�
                {
                    if(!isDead)
                    {
                        //�����Ŏ��S�ɂ��A���t���[����PlayOut
                        GManager.instance.DeadForce();
                        isDead = true;
                    }
                }
            }
            else
            {
                timeCount -= Time.deltaTime; //�o�ߎ��Ԃ����Z
                TimePlus(); //���ԉ񕜏���

                //20�b������ɂ���
                if(timeCount >= 20f)
                {
                    timeCount = 20f;
                }

                //�c�莞�Ԃ�10�b�𒴂��Ă���ꍇ�X�R�A�Ƀu�[�X�g��������
                if(timeCount >= 10f)
                {
                    GManager.instance.scoreBoost = timeCount * 0.1f;
                    timerText.color = Color.magenta; //�����F���}�[���^��
                    GManager.instance.uic.boostUI.SetActive(true);
                }
                else
                {
                    GManager.instance.scoreBoost = 1f;
                    timerText.color = Color.red; //�����F��Ԃ�
                    GManager.instance.uic.boostUI.SetActive(false);
                }

                timerText.text = timeCount.ToString("f2"); //�c�莞�Ԃ�\��
            }
        }
        else
        {
            if(!isResult)
            {
                if (countGOver >= 1f)
                {
                    GManager.instance.uic.ResultActive(); //���U���g�\��
                    isResult = true;
                }

                countGOver += Time.unscaledDeltaTime; //�^�C���X�P�[��0��Ԃł����Ԃ��J�E���g����
            }
        }
    }

    /// <summary>
    /// ���U���g�J�ڒ�����
    /// </summary>
    private void PlayOut()
    {
        //���Ԑ؂�AHP�؂ꂻ�ꂼ��ŃV�t�g���邽�߃��\�b�h�ɂ܂Ƃ߂�

        timerText.text = "GAME OVER";
        GManager.instance.uic.dScreenUI.SetActive(true);

        Time.timeScale = 0; //�Q�[���̎��Ԃ��~�߂�

        isGOver = true;
    }

    /// <summary>
    /// ���Ԃ̉��Z����
    /// </summary>
    private void TimePlus()
    {
        timeCount += GManager.instance.timeAd; //���Ԃ���
        GManager.instance.timeAd = 0; //���ԉ񕜗ʂ����Z�b�g
    }
}
