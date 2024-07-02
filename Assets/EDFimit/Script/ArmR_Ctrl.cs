using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�E��̃T�u�A�^�b�N�����p

public class ArmR_Ctrl : MonoBehaviour
{
    [Header("���ˈʒu")] public Transform firePos;
    [Header("���˕���")] public Transform shotDir;
    [Header("����SE")] public AudioClip shotSE;
    [Header("�e")] public GameObject cell;
    [Header("�A�ˑ��x")] public float shotInt;


    private bool isReShot = true; //�Ďˌ��\����
    private float reShotTime = 0.0f; //�ˌ���o�ߎ���
    private Vector3 shotDir2; //���˕���(�␳)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GManager.instance.noShot) //noShot��false�̎�
        {
            //�e�����J�[�\�������Ɍ�����
            transform.LookAt(GManager.instance.aimPos);

            //�ˌ�����
            if (isReShot) //�Ďˌ��\�ł���
            {
                if (Input.GetButton("Fire2")) //�ˌ��R�}���h�����͂��ꂽ(�E�N���b�N)
                {
                    //psMF.Play(); //�}�Y���t���b�V���Đ�
                    SoundManager.instance.PlaySE(shotSE); //�ˌ�SE��炷

                    shotDir2 = new Vector3(shotDir.eulerAngles.x + 90f, shotDir.eulerAngles.y, shotDir.eulerAngles.z);
                    Instantiate(cell, firePos.position, Quaternion.Euler(shotDir2)); //�e�I�u�W�F�N�g���o��

                    isReShot = false;

                }
            }
            else //�ˌ���C���^�[�o����
            {
                if (reShotTime > shotInt) //�ˌ���o�ߎ��Ԃ��ݒ肳�ꂽ�A�ˊԊu���ԂɒB����
                {
                    reShotTime = 0.0f; //�^�C�}�[���Z�b�g
                    isReShot = true; //�Ďˌ��\��
                }
                else
                {
                    reShotTime += Time.deltaTime; //�o�ߎ��Ԃ����Z
                }
            }
        }
    }
}
