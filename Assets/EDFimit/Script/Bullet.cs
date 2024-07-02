using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("���e�G�t�F�N�g")] public GameObject hitEf;

    private Rigidbody rb = null; //���W�b�h�{�f�B�擾
    private Vector3 shotVec = Vector3.zero; //�e�̐i�s����

    [SerializeField] S_AttackData bulletData;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shotVec = transform.up; //���ˎ��_�ł̐��ʂ�ۑ�(�e�I�u�W�F�N�g���c��̂��ߏ����)
    }

    private void FixedUpdate()
    {
        rb.velocity = shotVec * bulletData.speed; //�i�s�����փ��W�b�h�{�f�B����
    }

    /// <summary>
    /// ���e��
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            EnemyParameter ep = collision.gameObject.GetComponent<EnemyParameter>(); //�ڐG�����G�̃G�l�~�[�p�����[�^���擾

            Vector3 kbDirection = collision.contacts[0].point - transform.position; //�m�b�N�o�b�N�x�N�g�����v�Z
            ep.DamageCount(bulletData.atk, bulletData.kbForce, bulletData.kbTime, kbDirection); //�_���[�W�������Ăяo��(�_���[�W�A�m�b�N�o�b�N�́A�m�b�N�o�b�N����, �m�b�N�o�b�N�x�N�g��)
        }

        Instantiate(hitEf, this.transform.position, Quaternion.identity); //���e�G�t�F�N�g����(�ׂ������W�͗v����)
        Destroy(this.gameObject); //����
    }


    /// <summary>
    /// ��ʊO�ɏo���������
    /// </summary>
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
