using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HP : MonoBehaviour
{
    [Header("�A���_�[�o�[")] public Image ub;
    [Header("UI_HP_Dot")] public UI_HP_Dot[] uihpd;
    [Header("HP�h�b�g")] public Image[] hpD;
    [Header("HP�O���E")] public Image[] hpG;
    [Header("HP�e�L�X�g")] public TMP_Text hpText;

    private bool isStart; //�X�^�[�g���o��
    private bool isPlay; //�v���C��
    private bool isDamage; //�_���[�W������
    private int dn; //HP�h�b�g�̔ԍ�

    void Start()
    {
        isStart = true; //�A�N�e�B�u�ɂȂ�Ɠ����ɃX�^�[�g���o�J�n
        ub.fillAmount = 0f; //�A���_�[�o�[�̃t�B���A�}�E���g��0����J�n
    }

    void Update()
    {
        if(isPlay) //�v���C��
        {

        }
        else if(isStart) //�X�^�[�g���o��
        {
            if (ub.fillAmount >= 1f) //�A���_�[�o�[��1�܂ŐL�т�
            {
                ub.fillAmount = 1f; //������1�Ɋm��
                hpText.color = new Color(0, 0, 1, ub.fillAmount);
                hpD[0].color = new Color(0, 1, 0.78f, ub.fillAmount);
                hpD[1].color = new Color(0, 1, 0.78f, ub.fillAmount);
                hpD[2].color = new Color(0, 1, 0.78f, ub.fillAmount);
                hpG[0].color = new Color(1, 1, 1, ub.fillAmount);
                hpG[1].color = new Color(1, 1, 1, ub.fillAmount);
                hpG[2].color = new Color(1, 1, 1, ub.fillAmount);
                isPlay = true; //�v���C�������Ɉڍs
            }
            else
            {
                ub.fillAmount += Time.deltaTime; //�A���_�[�o�[��L�΂�

                //�A���_�[�o�[��̃e�L�X�g��C���[�W�̃A���t�@�l���A���_�[�o�[�̃t�B���A�}�E���g�ɘA�������ďグ��
                hpText.color = new Color(0, 0, 1, ub.fillAmount);
                hpD[0].color = new Color(0, 1, 0.78f, ub.fillAmount);
                hpD[1].color = new Color(0, 1, 0.78f, ub.fillAmount);
                hpD[2].color = new Color(0, 1, 0.78f, ub.fillAmount);
                hpG[0].color = new Color(1, 1, 1, ub.fillAmount);
                hpG[1].color = new Color(1, 1, 1, ub.fillAmount);
                hpG[2].color = new Color(1, 1, 1, ub.fillAmount);
            }
             
        }

        //���J�n��
        //�X�^�[�g�{�^���ɘA��(G�}�l�[�W���[�ɂ��A�N�e�B�u��)
        //�A���_�[�o�[���t�B���A�}�E���g�ō�����E�ɐL�т�
        //�L�т���A�������͓����ɁAHP�Ɓ�����������s�����ɑJ��

        //���v���C��
        //�_���[�W���ɉE���珇�Ɂ���������
        //�����ł͐��l�Ƃ��ĊǗ����Ă���
        //���������鎞�͐F���ς��(���͍����ۂ��A�����glow�����͐ԂɁH)
        //�F�ω���ɓ������ŏ�����
        //���ƃO���E�͔z��ŃA�N�Z�X���A�c��HP�l�Ɋ֘A�t���ē���������(���S�ɔ�\�����̕����ǂ��H)
    }

    public void HPDown()
    {
        dn = (int)GManager.instance.hpNow; //hpNow��float�^�Ȃ̂�int�^�ɕϊ�
        uihpd[dn].isVanish = true; //�Ή�HP�h�b�g�̏��Ńt���O���g�D���[��
                                   //���ڂ̔�e����hpNow��2�ɂȂ�AhpC[2]��3�ڂ�HP���ɃA�N�Z�X
    }

    /// <summary>
    /// HP����0
    /// </summary>
    public void HPDownAll()
    {
        uihpd[0].isVanish = true;
        uihpd[1].isVanish = true;
        uihpd[2].isVanish = true;
    }
}
