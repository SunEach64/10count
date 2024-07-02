using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Tracer : MonoBehaviour
{
    [Header("PlayerCtrl")] public PlayerCtrl pc;
    [Header("�Ǐ]�v���C���[")] public Transform target;

    private Vector3 cPos; //�x���Ǐ]����P�g���[�T�[���W
    private Vector3 cVel; //currentVelocity(�Ǐ]���x������)
    private float smTime = 0.2f; //�^�[�Q�b�g�ւ̖}���̓��B����(�ϓ�������)
    private float dafTime; //�_�b�V���㎞��
    private bool gCheck;
    private bool dashAf; //�_�b�V���㔻��

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pc.isDash) //�_�b�V����
        {
            dashAf = true;

            cPos = this.transform.position; //���ݍ��W��ۑ�
            cPos = Vector3.SmoothDamp(cPos, target.position, ref cVel, smTime); //�x���Ǐ]�̒l���v�Z

            this.transform.position = new Vector3(cPos.x, 1.05f, cPos.z);
        }
        else 
        {
            if(dashAf) //�_�b�V����
            {
                if(dafTime >= smTime)
                {
                    //���x�����Ԃ�0.2�ɐݒ肵�Ă��邽�߁A�_�b�V����0.2�b�o���J�������v���C���[�ɒǂ�������_�b�V���㏈�����I����
                    dashAf = false;
                    dafTime = 0f;
                }
                else //�_�b�V������x���Ǐ]�p��
                {
                    dashAf = true;

                    cPos = this.transform.position; //���ݍ��W��ۑ�
                    cPos = Vector3.SmoothDamp(cPos, target.position, ref cVel, smTime); //�x���Ǐ]�̒l���v�Z

                    this.transform.position = new Vector3(cPos.x, 1.05f, cPos.z);
                }
            }
            else //�ʏ�ړ���
            {
                this.transform.position = new Vector3(target.position.x, 1.05f, target.position.z);
            }
        }
        
    }
}
