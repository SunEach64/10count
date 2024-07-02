using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//�w��I�u�W�F�N�g�̍��W����ɃJ��������񂳂���

public class CameraCtrl2 : MonoBehaviour
{
    [Header("��]���x")] public float rollSpeed = 100f;

    [SerializeField] private CinemachineVirtualCamera cvc; //�o�[�`�����J����������ϐ�

    private float rotRL = 0; //��]����
    private float angBf; //��]�O�p�x
    private float angAf; //��]��p�x
    private bool isRoll; //��]���t���O

    CinemachineOrbitalTransposer cot; //�I�[�r�^���g�����X�|�[�U�[�𐧌䂷��ϐ�

    void Start()
    {
        cot = cvc.GetCinemachineComponent<CinemachineOrbitalTransposer>(); //�I�[�r�^���g�����X�|�[�U�[���擾
        angBf = cot.m_XAxis.Value; //�J�n���̃J�����p�x��ۑ�
    }

    void Update()
    {
        if(!isRoll) //��]���łȂ���
        {
            if (Input.GetKeyDown(KeyCode.E)) //E�L�[�������Ă���
            {
                angAf = angBf + 45; //��]��̊p�x������
                rotRL = 1;
                isRoll = true;

                //��]��p�x��ValueRange�𒴂�������360��������Range���ɖ߂�
                if(angAf >= cot.m_XAxis.m_MaxValue)
                {
                    angAf -= 360;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q)) //Q�L�[�������Ă���
            {
                angAf = angBf - 45; //��]��̊p�x������
                rotRL = -1;
                isRoll = true;

                //��]��p�x��ValueRange�𒴂�������360�𑫂���Range���ɖ߂�
                if(angAf <= cot.m_XAxis.m_MinValue)
                {
                    angAf += 360;
                }
            }
        }
        else //��]��
        {
            float rn = angAf - cot.m_XAxis.Value;

            if(rn >= 1f || rn <= -1f) //��]��Ƃ̊p�x����1�ȏ�
            {
                cot.m_XAxis.Value += rotRL * rollSpeed * Time.deltaTime; //��]��i�߂�
            }
            else //��]�I��
            {
                cot.m_XAxis.Value = angAf; //��]��p�x�Ɋm��
                rotRL = 0; //��]�������Z�b�g
                isRoll = false; //��]�t���O���Z�b�g
                angBf = angAf; //��]�O�p�x���X�V
            }
        }
        
    }
}
