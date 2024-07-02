using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�}�E�X�J�[�\���̈ʒu�ɏƏ���\��������
//�y�у��C�L���X�g�Ń}�E�X�|�C���g�ʒu�ɏƏ��������悤�ɂ���

public class CursorCtrl : MonoBehaviour
{
    [Header("�J�[�\���摜�ʒu")] public RectTransform cPos;
    [Header("�␳�L��Ə�")] public GameObject aimPos;

    [HideInInspector] public Vector3 cursorPos; //�J�[�\�����W(�v���C���[����)

    private Plane plane = new Plane(); //Plane���`�FCursorPoint�p
    private float distance = 0; //�����FCursorPoint�p
    private bool aimSupport; //�G�C���␳�t���O


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!GManager.instance.isGOver) //�Q�[���I�[�o�[��A���Z�b�g�O�ɂ����̏������~�߂Ȃ��ƃG���[���o��
        {
            if(GManager.instance.gc.IsGround()) //�ڒn��
            {
                plane.SetNormalAndPosition(Vector3.up, GManager.instance.player.transform.position); //�v���C���[����
            }
            else //��
            {
                plane.SetNormalAndPosition(Vector3.up, GManager.instance.pTracer.transform.position); //P�g���[�T�[����
            }

            CursorPoint(); //cursorPos�𖈃t���[���X�V
            transform.position = cursorPos; //�Ə��ʒu��cursorPos�Ɉړ�(ShotCtrl���ō������ēx�␳����)
        }
        
        cPos.position = Input.mousePosition; //�Ə��摜���ړ�
        
    }

    /// <summary>
    /// �v���C���[�����J�[�\���ʒu
    /// </summary>
    void CursorPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //���C���J��������}�E�X�J�[�\���̍��W��Ray���΂�

        if (plane.Raycast(ray, out distance))  //plane��Ray���������Ă��邩�𔻒�A����true�̎��͍X�Ɍ��_(�J����)��������_�܂ł̋������擾��distance�֓����
        {
            cursorPos = ray.GetPoint(distance); //�J�[�\���̍��W���`(Ray��distance�������W�A�܂�Plane��Ray�̌������W�A�܂�}�E�X�J�[�\���̈ʒu)
        }
        else
        {

        }

        //��Plane.Raycast()�FRay��Plane�ƌ������鎞true��Ԃ�
        //Physics.Raycast()�ƈႢPlane��Ώۂɂ��邱�Ƃ��猚���ȂǏ�Q�����ђʂ��Ĉʒu���擾�ł���
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            aimPos.transform.position = other.transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy")
        {
            aimPos.transform.position = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            aimPos.transform.position = this.transform.position;
        }
    }
}
