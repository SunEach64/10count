using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�|�[�Y�����{�^��

public class UI_ResumeButton : MonoBehaviour
{
    private bool isPush; //�{�^�������ꂽ����
    private float timer; //�{�^�����o�^�C�}�[
    private RectTransform rt; //�{�^���̃��N�g�g�����X�t�H�[��

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isPush)
        {
            //���Ԍo�߂Ń{�^������������������(�����ꂽ���o)
            //���o��ɃQ�[���J�n����

            if (timer >= 0.2f) //�{�^�����o���ԏI��(���Ԃ͗v����)
            {
                isPush = false; //�{�^���������胊�Z�b�g
                timer = 0f; //�^�C�}�[���Z�b�g
                rt.localScale = Vector3.one; //�{�^���T�C�Y���Z�b�g
                Time.timeScale = 1; //�^�C���X�P�[��1�ɕύX
                GManager.instance.noShot = false; //�ˌ��\
                GManager.instance.uic.PauseOut(); //�|�[�Y��ʉ���
            }

            timer += Time.unscaledDeltaTime;
        }
    }

    public void OnPressed()
    {
        isPush = true;
        rt.localScale = rt.localScale * 0.9f;
    }
}
