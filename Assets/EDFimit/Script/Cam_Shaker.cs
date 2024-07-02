using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//Vcam��h�炷����
//��Ƀ_���[�W���̉�ʐU��

public class Cam_Shaker : MonoBehaviour
{
    private CinemachineImpulseSource impulse;

    void Start()
    {
        impulse = GetComponent<CinemachineImpulseSource>();
    }

    /// <summary>
    /// �U�����s
    /// </summary>
    public void ShakeOn()
    {
        //G�}�l�[�W���[����Ăяo��
        impulse.GenerateImpulse();
    }
}
