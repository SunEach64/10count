using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Q�[���}�l�[�W���[

public class GManager : MonoBehaviour
{
    public static GManager instance = null;

    [Header("�v���C���[")] public GameObject player;
    [Header("�v���C���[�ڒn����")] public GroundCheck gc;
    [Header("P�g���[�T�[")] public GameObject pTracer;
    [Header("�Ə��J�[�\��")] public Transform cursorPos;
    [Header("�␳�L��Ə�")] public Transform aimPos;
    [Header("MAP���S")] public Transform mapCenter;
    [Header("UI_HP")] public UI_HP uihp;
    [Header("��ʐU��")] public Cam_Shaker camShake;
    [Header("�G���j�J�E���g")] public EnemyCount ec; //�e�G�l�~�[�ƃ��U���g���Ăяo��
    [Header("UI_Ctrl")] public UI_Ctrl uic;

    [HideInInspector] public bool noShot; //�ˌ��s����(���S���A�|�[�Y���Ȃ�)
    [HideInInspector] public bool isDam; //��e����������
    [HideInInspector] public bool isDam_uihp; //UI_HP�ւ̎w�ߗp
    [HideInInspector] public bool pDeath; //���S����
    [HideInInspector] public bool isGOver; //�Q�[���I�[�o�[�t���O(��ɊO������̓ǂݎ��p)
    [HideInInspector] public float eAtk; //�G�U����
    [HideInInspector] public float hpBase; //�ő�HP��b�l
    [HideInInspector] public float hpMax; //�ő�HP�l
    [HideInInspector] public float hpNow; //����HP�l
    [HideInInspector] public float hpRate; //�cHP����
    [HideInInspector] public float pAtk; //�v���C���[�U����
    [HideInInspector] public float score; //���j�X�R�A
    [HideInInspector] public float timeAd; //���ԉ񕜒l(�X�R�A�l�����ɒ~�ς���A�^�C�}�[���Ăяo���l������Ď��Ԃ��񕜂���) 
    [HideInInspector] public float scoreBoost; //�X�R�A�{��(�^�C�}�[����␳�����󂯎��)
    [HideInInspector] public int destCount; //���j��


    void Awake()
    {
        //�Q�[���}�l�[�W���[���V���O���g���ɂ���
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
            //�p�u���b�N�ϐ��ŃA�^�b�`���Ă����e��C���X�^���X���V�[���ă��[�h���ɃG���[�̌����ƂȂ��Ă��邽�߁A
            //�Ƃ肠��������Ɋւ��Ă̓Q�[���}�l�[�W���[�̃V�[���ׂ��͒��߂�(�K�v�Ȃ�ʂȃQ�[���}�l�[�W���[�����߂č�邪�A��������������邽�ߍŏ����ɍi��)
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0; //���Ԃ��~�߂�
            noShot = true; //�ˌ��s��
            uic.PauseIn(); //�|�[�Y��ʂ�
        }
    }


    /// <summary>
    /// �J�n����HP�ݒ�
    /// </summary>
    public void HPActive()
    {
        //HP�l��ݒ�(���͂܂���b�l�̂�)
        hpNow = hpBase;
        hpMax = hpBase;
    }


    /// <summary>
    /// ��_���[�W���������X
    /// </summary>
    public void PHPDown()
    {
        //�v���C���[�X�N���v�g����Ăяo�����

        hpNow = hpNow - eAtk; //��������
        uihp.HPDown(); //UI_HP�̌��������Ăяo��
        Instantiate(uic.damef, uic.damefPos.position, Quaternion.Euler(uic.damefRot.eulerAngles), uic.damefPos); //�_���[�W�G�t�F�N�g�o��(Canvas2�̎q�I�u�W�F�N�g�ɂ���)
        camShake.ShakeOn(); //��ʐU�����o

        if(hpNow <= 0f)
        {
            uic.rScreenUI.SetActive(true); //���b�h�X�N���[�����o�J�n
            noShot = true; //�ˌ��s��
        }
    }

    /// <summary>
    /// �������S
    /// </summary>
    public void DeadForce()
    {
        hpNow = 0; //HP��������0��
        uihp.HPDownAll(); //UI_HP�̌��������Ăяo��(����0)
        camShake.ShakeOn(); //��ʐU�����o
        uic.rScreenUI.SetActive(true); //���b�h�X�N���[�����o�J�n
        noShot = true; //�ˌ��s��
    }


    /// <summary>
    /// �U���͕ύX
    /// </summary>
    /// <param name="a"></param>
    public void PAtkChange(float pa)
    {
        pAtk = pa;
    }

    /// <summary>
    /// �G�U���͌���
    /// </summary>
    /// <param name="ea"></param>
    public void EAtkCange(float ea)
    {
        if(!isDam)
        {
            eAtk = ea;
            isDam = true; //�_���[�W�������ɒǉ��ŐڐG���Ă��_���[�W�ʂ��ς��Ȃ��悤�ɂ���
        }
        
    }

    /// <summary>
    /// �G�̍U����
    /// </summary>
    /// <returns></returns>
    public float EAtk()
    {
        return eAtk;
    }

    /// <summary>
    /// �X�R�A�Ǘ�
    /// </summary>
    public void ScoreCount(float enScore)
    {
        score += enScore * scoreBoost; //��b�X�R�A�~�c�莞�Ԃɂ��u�[�X�g���A��b�X�R�A�͓G���Ƃɐݒ肳��AenScore�����Ŏ擾
        destCount++; //���j�J�E���g���Z
        timeAd++; //���ԉ񕜒l���Z
    }

    /// <summary>
    /// �}�b�v���S���W
    /// </summary>
    /// <returns></returns>
    public Vector3 MapCenter()
    {
        return mapCenter.position;
    }


    //�A�C�e���̊l������
    //�v���C���[�̃A�C�e���`�F�b�N���A�C�e���ɐڐG
    //�A�C�e���`�F�b�N��IsItem()���g�D���[��
    //�v���C���[��Update��IsItem()�𔻒肵�A�g�D���[�Ȃ�G�}�l�[�W���[���Ăяo��
    //�v���C���[��Update�̔��莞�͈���擾�������s���悤�ɂ���
    //G�}�l�[�W���[���Ăяo���A�C�e���l�����̃��\�b�h������
    //G�}�l�[�W���[���A�C�e���̏����擾
    //���̍ۃQ�[���I�u�W�F�N�g���̂�GameObject�ϐ��Ŏ擾����(�Ō��Destroy�ŏ�������)
    //�擾�����A�C�e���̃p�����[�^��G�}�l�[�W���[�ŊǗ����Ă���X�e�[�^�X�ɉ�����
    //����������������Destroy�ŃA�C�e��������
    //
    //��(�ϐ�) = other.gameObject�ŃR���W���������I�u�W�F�N�g���擾�H
}
