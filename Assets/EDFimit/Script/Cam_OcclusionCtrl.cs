using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�J��������v���C���[�֒Ǐ]����Ǔ������g���K�[

public class Cam_OcclusionCtrl : MonoBehaviour
{
    [Header("�J����")] public GameObject cam;
    [Header("�^�[�Q�b�g")] public GameObject target; //�v���C���[��P�g���[�T�[�H


    void FixedUpdate()
    {
        this.gameObject.transform.position = cam.transform.position;
        //this.transform.LookAt(target.transform.position);
    }

}
