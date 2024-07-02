using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�T�u�A�^�b�N�p�̒e(��������)
//�����͊T�˃o���b�g�ɏ��������i

public class AttackB_Cell : MonoBehaviour
{
    [Header("�����T�u�A�^�b�N")] public GameObject subAttack;

    private Rigidbody rb = null; //���W�b�h�{�f�B�擾
    private Vector3 shotVec = Vector3.zero; //�e�̐i�s����

    [SerializeField] S_AttackData aData; //��{�I�ɂ͐��������T�u�A�^�b�N�{�̂Ɠ����X�N���v�^�u���I�u�W�F�N�g���g����

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shotVec = transform.up; //���ˎ��_�ł̐��ʂ�ۑ�(�e�I�u�W�F�N�g���c��̂��ߏ����)
    }

    void FixedUpdate()
    {
        rb.velocity = shotVec * aData.speed; //�i�s�����փ��W�b�h�{�f�B����
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //�v���C���[�����̓��C���[�Ŕ�ڐG�ɂ��A���͓G�ł������ł��ڐG������쓮
        Instantiate(subAttack, this.transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }
}
