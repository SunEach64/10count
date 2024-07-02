using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ArertMessage : MonoBehaviour
{
    [Header("�g")] public Image frame;
    [Header("RectTransform")] public RectTransform rt;

    private int blinkCount; //�_�ŉ�
    private float scaleY; //�A���[�g���b�Z�[�W�o������Y�X�P�[��
    private float al = 1f; //�t���[�����Ŏ��̃A���t�@�l(1����J�n)
    private bool alUp; //���Œ��̃t���O�ؑ֗p
    private bool ef1; //���o1�F�o��
    private bool ef2; //���o2�F����
    private bool ef3; //���o3�F����

    void Start()
    {
        ef1 = true; //���o1�J�n
    }
    void Update()
    {
        if(ef1)
        {
            ArertEf1();
        }
        else if(ef2)
        {
            ArertEf2();
        }
        else if(ef3)
        {
            ArertEf3();
        }
    }

    /// <summary>
    /// ���b�Z�[�W�o�����o
    /// </summary>
    private void ArertEf1()
    {
        if(scaleY >= 1)
        {
            scaleY = 1;

            //�t���O�ؑ�
            ef1 = false;
            ef2 = true;
        }
        else
        {
            scaleY += Time.deltaTime * 2; //Y�X�P�[�����グ��
        }

        rt.localScale = new Vector3(1, scaleY, 1); //�T�C�Y��
    }

    /// <summary>
    /// �t���[�����ŉ��o
    /// </summary>
    private void ArertEf2()
    {
        if(blinkCount < 2) //2��_�ł���܂�
        {
            if(!alUp)
            {
                al -= Time.deltaTime * 2; //�A���t�@�l��������

                if (al <= 0.2f) //�A���t�@�l��0.2���������
                {
                    alUp = true; //�t���O�ؑ�
                }
            }
            else 
            {
                al += Time.deltaTime * 2; //�A���t�@�l���グ��

                if(al >= 1f) //�A���t�@�l��1�ɒB����
                {
                    alUp = false; //�t���O�ؑ�
                    blinkCount += 1; //���ŃJ�E���g+1
                }
            }
        }
        else
        {
            al = 1;

            //�t���O�ؑ�
            ef2 = false;
            ef3 = true;
            blinkCount = 0; //�J�E���g���Z�b�g
        }
        
        frame.color = new Color(1, 0, 0, al); //���ߓx����
    }


    private void ArertEf3()
    {
        if (scaleY <= 0)
        {
            scaleY = 0;

            //�t���O�ؑ�
            ef3 = false;
            ef1 = true;

            //G�}�l�[�W���[����Z�b�g�A�N�e�B�u���Ăяo��
            GManager.instance.uic.ArertOff();
        }
        else
        {
            scaleY -= Time.deltaTime * 2; //Y�X�P�[����������
        }

        rt.localScale = new Vector3(1, scaleY, 1); //�T�C�Y��
    }
}
