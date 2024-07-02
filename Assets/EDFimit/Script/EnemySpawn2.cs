using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�G�X�|�i�[
//��ʊO�̃����_���ʒu�ɐ���
//�{�X������ŏo���悤�ɂ��Ă��܂�

public class EnemySpawn2 : MonoBehaviour
{
    [Header("�G���G�v���n�u")] public GameObject enemy;
    [Header("�{�X�G�v���n�u")] public GameObject boss;
    [Header("�X�|�[���|�C���g")] public GameObject[] esp;
    [Header("�G�����")] public int enLimit = 50; //�f�t�H���g50�Ƃ���

    private GameObject[] tagObj; //�G�̐��J�E���g�p
    private GameObject spnEnemy; //�o��������G�L����(�����ɉ����ĎG���G�ƃ{�X�G��ؑ�)
    private int enQty; //�G�̐�
    private int num; //�X�|�[���|�C���g�̔ԍ�
    private float interval; //�ďo���܂ł̃C���^�[�o��
    private float scoreNow; //���݃X�R�A(G�}�l�[�W���[����擾)
    private float nextSpawn; //���̃{�X�X�|�[���X�R�A

    void Start()
    {
        nextSpawn = 1000f; //��̖ڂ̃{�X��1000�X�R�A�ŏo���Ƃ���
    }

    void Update()
    {

        transform.position = GManager.instance.pTracer.transform.position; //�X�|�i�[�̈ʒu�X�V
        //�X�|�i�[�̈ʒu��P�g���[�T�[(�ꏏ�ɉ�]����̂�����邽�߃v���C���[�̎q�ɂ͂��Ȃ�)

        scoreNow = GManager.instance.score; //G�}�l�[�W���[���猻�݃X�R�A���擾

        if (scoreNow >= nextSpawn) //�{�X�o���X�R�A�ɒB���Ă��邩�𔻒�
        {
            spnEnemy = boss; //�o���G���{�X�ɐݒ�
        }
        else
        {
            spnEnemy = enemy; //�o���G���G���G�ɐݒ�
        }

        if (interval >= 0.5f)
        {
            QtyCheck("Enemy"); //Enemy�^�O�̐��𔻒�

            if(enQty <= enLimit) //��������B
            {
                ESpawn(); //�G��ǉ��X�|�[��
            }
        }
        else
        {
            interval += Time.deltaTime;
        }

    }

    private void ESpawn()
    {
        //�X�|�[���|�C���g������
        num = Random.Range(0, esp.Length);
        Vector3 espPos = esp[num].transform.position;

        //�X�|�[���|�C���g�ɃR���C�_�[�����邩�𔻒�(����͔��a2.5�̋���)
        //����ʒu���Ⴂ�Ə��̃R���C�_�[�ɓ����邽�ߍ��߂ɂ���
        if (Physics.OverlapSphere(new Vector3(espPos.x, 2.6f, espPos.z), 2.5f).Length > 0) 
        {
            //�������Ȃ�
        }
        else
        {
            //�G�𐶐�
            Instantiate(spnEnemy, new Vector3(espPos.x, 1.1f, espPos.z), Quaternion.identity);
            interval = 0; //�����o�����������^�C�}�[�����Z�b�g

            if(spnEnemy == boss) //�o�������G���{�X�������ꍇ
            {
                GManager.instance.uic.ArertOn(); //�A���[�g���b�Z�[�WON

                nextSpawn += 1000f; //���̃{�X�o����+1000(�b��)
            }
        }
    }

    /// <summary>
    /// �G�l�~�[���m�F
    /// </summary>
    private void QtyCheck(string tagname)
    {
        tagObj = GameObject.FindGameObjectsWithTag(tagname); //tagname�ɓ��ꂽ�^�O���̃I�u�W�F�N�g��T��

        enQty = tagObj.Length; //�������G��
    }
}
