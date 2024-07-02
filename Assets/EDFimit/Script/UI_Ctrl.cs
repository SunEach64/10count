using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Canvas�̕\���Ǘ�
//���G�}�l�[�W���[�o�R�ŌĂяo������

public class UI_Ctrl : MonoBehaviour
{
    [Header("�v���C��UI")] public GameObject playUI;
    [Header("�X�^�[�g���UI")] public GameObject startUI;
    [Header("���U���gUI")] public GameObject resultUI;
    [Header("���Z�b�g�{�^��UI")] public GameObject resetUI;
    [Header("�T�E���hUI")] public GameObject soundUI;
    [Header("�|�[�Y���UI")] public GameObject pauseUI;
    [Header("���X�N���[��UI")] public GameObject dScreenUI;
    [Header("�ԃX�N���[��UI")] public GameObject rScreenUI;
    [Header("�X�R�AUI")] public GameObject ScoreUI;
    [Header("�X�R�A�u�[�X�g")] public GameObject boostUI; //�^�C�}�[����SetActive�𐧌䂷��
    [Header("�_���[�W�G�t�F�N�g")] public GameObject damef;
    [Header("�_���[�W�G�t�F�N�g�ʒu")] public Transform damefPos; //�e�I�u�W�F�N�g�ɂ���Canvas2
    [Header("�_���[�W�G�t�F�N�g�p�x")] public Transform damefRot; //���C���J�����̊p�x
    [Header("�A���[�gUI")] public GameObject arertUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �J�n�O���
    /// </summary>
    public void PlayStay()
    {
        //�v���C���[���\��
        GManager.instance.player.SetActive(false);
        //��\��UI��ݒ�
        playUI.SetActive(false);
        resultUI.SetActive(false);
        resetUI.SetActive(false);
        rScreenUI.SetActive(false);
    }

    /// <summary>
    /// �X�^�[�g�{�^������
    /// </summary>
    public void PlayStart()
    {
        //�v���C���[���A�N�e�B�u
        GManager.instance.player.SetActive(true);
        //UI���A�N�e�B�u�A�X�^�[�g�{�^���Ɖ���UI���\��
        playUI.SetActive(true);
        startUI.SetActive(false);
        soundUI.SetActive(false);
    }

    /// <summary>
    /// �����߉�ʉ���
    /// </summary>
    public void DarkScreenOut()
    {
        dScreenUI.SetActive(false);
    }


    /// <summary>
    /// ���U���g��ʕ\��
    /// </summary>
    public void ResultActive()
    {
        ScoreUI.SetActive(false);
        resultUI.SetActive(true);
        GManager.instance.isGOver = true;
    }


    /// <summary>
    /// ���U���g�F���Z�b�g�{�^���\��
    /// </summary>
    public void ResetActive()
    {
        resetUI.SetActive(true);
    }


    /// <summary>
    /// �A���[�g���b�Z�[�WON
    /// </summary>
    public void ArertOn()
    {
        arertUI.SetActive(true);
    }

    /// <summary>
    /// �A���[�g���b�Z�[�WOFF
    /// </summary>
    public void ArertOff()
    {
        arertUI.SetActive(false);
    }

    /// <summary>
    /// �|�[�Y���
    /// </summary>
    public void PauseIn()
    {
        pauseUI.SetActive(true);
        resetUI.SetActive(true);
        soundUI.SetActive(true);
        dScreenUI.SetActive(true);
    }

    /// <summary>
    /// �|�[�Y��ʉ���
    /// </summary>
    public void PauseOut()
    {
        pauseUI.SetActive(false);
        resetUI.SetActive(false);
        soundUI.SetActive(false);
        //��dScreenUI�̓^�C���X�P�[���𓮂����Ώ���ɓ������������
    }

}
