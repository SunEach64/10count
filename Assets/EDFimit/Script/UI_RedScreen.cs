using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���U���g��ʂł̔������ԃX�N���[��

public class UI_RedScreen : MonoBehaviour
{
    [Header("�}�e���A��")] public Material rs;

    private float alpha; //�}�e���A���̃A���t�@�l(���ߗ�)

    void Start()
    {
        
    }

    void Update()
    {
        if (alpha >= 0.7f)
        {
            rs.SetColor("_Color", new Color(0.2f, 0, 0, 0.7f));
        }
        else
        {
            alpha += 0.2f * Time.deltaTime; //���Ԍo�߂ŃA���t�@�l���グ�Ă���

            rs.SetColor("_Color", new Color(0.2f, 0, 0, alpha));
        }


    }
}
