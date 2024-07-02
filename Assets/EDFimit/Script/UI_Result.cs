using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Result : MonoBehaviour
{
    [Header("�X�R�A")] public TMP_Text txScore;
    [Header("���j��")] public TMP_Text txCount;
    [Header("�ʌ��j��")] public string[] txInd; //�ʂ̌��j�\�L

    private int loop; //���[�v��
    private int countQty; //txCount�̍��ڐ�
    private int zombieCount;
    private int bossZCount;
    private float timer;
    private bool afRslt; //���U���g�����t���O
    private bool rslt0; //�X�R�A�\��
    private bool rslt1; //�J�E���g�\��
    private bool reset; //���Z�b�g�{�^���\����

    void Start()
    {
        txCount.text = null; //��x��ɂ���
        ECount(); //�e���j�����擾
        rslt0 = true;
    }

    void Update()
    {
        //���j���J�E���g�̕\����0.5�b�����݂ŕ\��
        //�S�̃X�R�A�\����0.5�b���]���r�\����0.5�b���{�X�]���r�\����0.5�b���c
        //���j�����G��ނɉ����ă��[�v�ōs��������H

        if(!afRslt)
        {
            if (rslt0)
            {
                txScore.text = "Result\nSCORE : " + GManager.instance.score.ToString("f0"); //�X�R�A�\��
                rslt0 = false;
                rslt1 = true;
                timer = 0; //�^�C�}�[���Z�b�g
            }
            if (rslt1 && timer >= 0.5f) //0.5�b���݂œG���j����\�����Ă���
            {
                txCount.text = txCount.text + "\n" + txInd[loop] ; //���j�������̍s�ɒǉ�

                if (loop < countQty) //���[�v�p��
                {
                    loop++;
                    timer = 0; //�s�x�^�C�}�[�����Z�b�g
                }
                else //���[�v�����ă��Z�b�g�{�^���\����
                {
                    rslt1 = false;
                    reset = true;
                    timer = 0;
                }

            }
            if (reset && timer >= 0.5f) //���Z�b�g�{�^���\��
            {
                GManager.instance.uic.ResetActive(); //���Z�b�g�{�^���\��
                reset = false;
                afRslt = true;
            }

            timer += Time.unscaledDeltaTime;
        }
    }

    private void ECount()
    {
        //�Ή�����G�ԍ���EnemyCount�X�N���v�g�Q��
        //1�̈ȏ�|���Ă����ꍇ�AcountQty��1�����Ă��̔ԍ��̃e�L�X�g�Ɋ���U����
        //��ʂ�W�v��AcountQty���|�����G��ސ��ɂȂ邽�߁A��������[�v�̎��s�񐔂ɂ���H

        zombieCount = GManager.instance.ec.eCount[0]; //�]���r�̌��j�����擾
        if(zombieCount >= 0)
        {
            //�Ƃ肠�����ʏ�]���r�̃��U���g�\�L�͌��j0�̂ł��\��������
            txInd[0] = "Zombie : " + zombieCount;
        }
        bossZCount = GManager.instance.ec.eCount[1]; //�{�X�]���r�̌��j�����擾
        if(bossZCount > 0)
        {
            countQty += 1;
            txInd[countQty] = "BossZombie : " + bossZCount;
        }
    }
}
