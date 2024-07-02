using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//HP�h�b�g�X�̋���
//��Ƀ_���[�W���̏��ŉ��o

public class UI_HP_Dot : MonoBehaviour
{
    [HideInInspector] public bool isVanish; //�h�b�g�̏��Ńt���O�AUI_HP���琧��

    private float al = 1f; //�A���t�@�l
    private Image im;

    // Start is called before the first frame update
    void Start()
    {
        im = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isVanish)
        {
            if(al <= 0f)
            {
                al = 0f;
                isVanish = false; //���Ńt���O���I�t�ɂ��Ĉȍ~�̖��ʂȏ������Ȃ�
            }
            else
            {
                al -= Time.deltaTime; //�o�ߎ��Ԃ����Z
                im.color = new Color(0, 0, 0.78f, al); //�h�b�g�𓧖��ɂ��Ă���
            }
        }
    }
}
