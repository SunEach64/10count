using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����ɃA�^�b�`����A�e�̔��˗p

public class ShotCtrl : MonoBehaviour
{
    [Header("�e���ʒu")] public Transform firePos;
    [Header("���˕���")] public Transform shotDir;
    [Header("�ˌ�SE")] public AudioClip shotSE;
    [Header("�e��")] public GameObject bullet;
    [Header("�A�ˑ��x")] public float shotInt;
    [Header("�}�Y���t���b�V��")] public ParticleSystem psMF;

    private bool isReShot = true; //�Ďˌ��\����
    private float reShotTime = 0.0f; //�ˌ���o�ߎ���
    private Vector3 shotDir2; //���˕���(�␳)

    // Start is called before the first frame update
    void Start()
    {
        psMF.Stop(); //�J�n���̓p�[�e�B�N���V�X�e�����~�߂Ă���
    }

    void Update()
    {
        if(!GManager.instance.noShot) //noShot��false�̎�
        {
            //�e�����J�[�\�������Ɍ�����
            transform.LookAt(GManager.instance.aimPos);

            //�ˌ�����
            if (isReShot) //�Ďˌ��\�ł���
            {
                if (Input.GetButton("Fire1")) //�ˌ��R�}���h�����͂��ꂽ
                {
                    psMF.Play(); //�}�Y���t���b�V���Đ�
                    SoundManager.instance.PlaySE(shotSE); //�ˌ�SE��炷

                    shotDir2 = new Vector3(shotDir.eulerAngles.x + 90f, shotDir.eulerAngles.y, shotDir.eulerAngles.z);
                    Instantiate(bullet, firePos.position, Quaternion.Euler(shotDir2)); //�e�I�u�W�F�N�g���o��

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
