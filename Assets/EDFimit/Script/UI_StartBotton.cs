using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�X�^�[�g�{�^���̋���

public class UI_StartBotton : MonoBehaviour
{
    [Header("�X�^�[�g��")] public AudioClip seStart;

    private bool isPush; //�{�^�������ꂽ����
    private float timer; //�{�^�����o�^�C�}�[
    private RectTransform rt; //�{�^���̃��N�g�g�����X�t�H�[��

    void Start()
    {
        rt = GetComponent<RectTransform>();

        Time.timeScale = 0; //�J�n�O�͎��Ԃ��~�߂�
        GManager.instance.uic.PlayStay(); //�J�n�O���
    }

    // Update is called once per frame
    void Update()
    {
        if(isPush)
        {
            //���Ԍo�߂Ń{�^������������������(�����ꂽ���o)
            //���o��ɃQ�[���J�n����

            if(timer >= 0.2f) //�{�^�����o���ԏI��(���Ԃ͗v����)
            {
                Time.timeScale = 1; //�^�C���X�P�[�������Z�b�g(���g���C����0�̂܂܂ɂȂ邽��)

                //�v���C���[�A�G�A�v���CUI���A�N�e�B�u�A�X�^�[�g�{�^�����\����
                GManager.instance.uic.PlayStart();
            }

            timer += Time.unscaledDeltaTime;
        }
    }

    public void OnPressed()
    {
        isPush = true;
        SoundManager.instance.PlaySE(seStart);
        rt.localScale = rt.localScale * 0.9f;
    }
}
