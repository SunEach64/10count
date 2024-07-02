using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Q�[���X�^�[�g���A���̔�������ʂ����Ԍo�߂œ���������

public class UI_DarkScreen : MonoBehaviour
{
    [Header("�}�e���A��")] public Material ds;

    private float alpha; //�}�e���A���̃A���t�@�l(���ߗ�)
    private bool isStart; //���񋓓��t���O

    void Start()
    {
        
    }

    void Update()
    {
        if (!isStart) //���񋓓�
        {
            isStart = true;
            alpha = 0.5f; //�������ɐݒ�
        }
        else //���o����
        {
            alpha -= Time.deltaTime; //���Ԍo�߂ŃA���t�@�l�������Ă���

            ds.SetColor("_BaseColor", new Color(0, 0, 0, alpha));

            if (alpha <= 0)
            {
                isStart = false; //���񋓓��t���O�����Z�b�g
                GManager.instance.uic.DarkScreenOut(); //��\������
            }
        }
        
    }
}
