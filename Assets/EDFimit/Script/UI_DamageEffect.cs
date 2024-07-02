using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�_���[�W���󂯂��ۂ̉�ʂ��Ԃ��Ȃ����
//�v���n�u��G�}�l�[�W���[�o�R��Instantiate�ŌĂяo���ƈ�񖾖�(�A���t�@�l1�ŏo�����������H)���f�X�g���C

public class UI_DamageEffect : MonoBehaviour
{
    [Header("����")] public Image efL;
    [Header("�E��")] public Image efR;

    private float al = 1f; //�A���t�@�l

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(al <= 0) //����������
        {
            Destroy(this);
        }
        else
        {
            efL.color = new Color(1, 0, 0, al); //�A���t�@�l���X�V
            efR.color = new Color(1, 0, 0, al); //�A���t�@�l���X�V

            al -= Time.deltaTime; //�o�ߎ��ԕ����Z
        }
    }
}
