using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����{��
//����U���p�̒e���ڐG����Ɛ��������

public class AttackB_Explosion : MonoBehaviour
{
    [SerializeField] S_AttackData aData;

    private SphereCollider sCol;
    private float timer;

    void Start()
    {
        sCol = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //���Ԍo�߂ŃR���C�_�[���I�t���f�X�g���C
        if (timer >= 3f)
        {
            Destroy(gameObject);
        }
        else if (timer >= 0.5f)
        {
            sCol.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            // SphereCollider�̒��S�����[���h���W�Ŏ擾
            Vector3 sphereCenterWorldPosition = transform.TransformPoint(sCol.center);

            // �ڐG�����I�u�W�F�N�g�̈ʒu���擾
            Vector3 otherPosition = other.transform.position;

            // SphereCollider�̒��S����ڐG�����I�u�W�F�N�g�ւ̃m�b�N�o�b�N�x�N�g�����v�Z
            Vector3 directionVector = otherPosition - sphereCenterWorldPosition;

            EnemyParameter ep = other.GetComponent<EnemyParameter>(); //�ڐG�����G�̃G�l�~�[�p�����[�^���擾
            ep.DamageCount(aData.atk, aData.kbForce, aData.kbTime, directionVector); //�_���[�W�������Ăяo��(�_���[�W�A�m�b�N�o�b�N�́A�m�b�N�o�b�N����, �m�b�N�o�b�N�x�N�g��)

            sCol.enabled = false;
        }
    }
}
