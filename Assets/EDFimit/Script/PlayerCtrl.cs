using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//�v���C���[�L�����̑���
//�����낵���_�Ń}�E�X�J�[�\�������������AQ/E�L�[�Ŏ��_����]�AWASD�ňړ�

public class PlayerCtrl : MonoBehaviour
{
    [Header("MainUnit")] public GameObject mu;
    [Header("HP")] public float hp; //�L�����N�^�[�ʂ̊�b�ő�HP�l
    [Header("�ړ����x")] public float runSpeed;
    [Header("�_�b�V����")][Range(0, 50)] public float dashSpeed;
    [Header("�W�����v��")] public float jumpSpeed;
    [Header("�W�����v������")] public float jumpAccel; //�W�����v���̉������ւ̉����␳
    [Header("�W�����v�J�[�u")] public AnimationCurve jumpCurve;
    [Header("�W�����v����]�l���")] public float jumpSpin;
    [Header("�d�͒l")] public float graSpeed;
    [Header("�����낵�������")] public GameObject lookPosN0;
    [Header("���C���J����")] public GameObject mainCam;
    [Header("�ڒn����")] public GroundCheck gc;
    [Header("�W�����vSE")] public AudioClip seJump;
    [Header("��eSE")] public AudioClip seDamage;
    [Header("���SSE")] public AudioClip seDead;
    [Header("���S�G�t�F�N�g")] public GameObject efDead;

    [HideInInspector] public bool isDash; //�_�b�V��������FP�g���[�T�[����ǂݎ�点�邽�߃p�u���b�N�ɂ���

    private Rigidbody rb; //���W�b�h�{�f�B�擾�p
    private GameObject hitEnemy; //�ڐG�����G	
    private Vector3 moveVel; //�ړ����x�l
    private Vector3 wsVel; //�O��ړ��l
    private Vector3 adVel; //���E�ړ��l
    private Vector3 vVel; //�����ړ���(�W�����v�㏸�l�Əd�͗����l�̍��v)
    private Vector3 jumpSpinVel; //�W�����v����]��
    private Vector3 dashSpinVel; //�_�b�V������]
    private Quaternion dashSpin; //�_�b�V�����̉�]���s���N�H�[�^�j�I��
    private float dashPow; //�_�b�V����2
    private float dashTime; //�_�b�V������
    private float jumpTime; //�W�����v����
    private float kbTime; //�m�b�N�o�b�N����
    private float deathTime; //����ł��烊�U���g�ɉf��܂ł̃^�C�����O
    private bool gCheck; //�ڒn����
    private bool isKnockback = false; //�m�b�N�o�b�N����
    private bool isDamage = false; //�_���[�W����(HP��������)
    private bool isDeath; //���S����
    private bool isJump; //�W�����v������
    private bool isJumpDown; //�W�����v�㗎������
    private bool dashAf; //�_�b�V���ォ��ă_�b�V���\�̔���(false�̎��Ƀ_�b�V���\)
    private int layerP; //�v���C���[�̃��C���[
    private int layerE; //�G�̃��C���[

   
    void Start()
    {
        //�R���|�[�l���g�擾
        rb = GetComponent<Rigidbody>();
        //GetComponent<Rigidbody>().maxAngularVelocity = 100f;

        GManager.instance.hpBase = hp; //G�}�l�[�W���[�̍ő�HP�x�[�X�l�ϐ��ɂ��̃L������HP�l������
        GManager.instance.HPActive(); //HP��ݒ�
        layerP = LayerMask.NameToLayer("Player"); //�v���C���[�̃��C���[�擾
        layerE = LayerMask.NameToLayer("Enemy"); //�G�l�~�[�̃��C���[�擾
        //layerP = 8;
        //layerE = 9;
    }
    
    void FixedUpdate()
    {
        gCheck = gc.IsGround(); //�ڒn��Ԏ擾

        if (!isDeath && GManager.instance.hpNow > 0) //���S���肪�t�@���X����HP��0�ł͂Ȃ�
        {
            if (gCheck && !isKnockback) //�ڒn����g�D���[�A���m�b�N�o�b�N����t�@���X(�ʏ펞����)
            {
                jumpAccel = 1f; //�W�����v���ȊO�͉����␳��؂�

                if(isJumpDown) //�W�����v���n��ɐڒn
                {
                    isJumpDown = false; //������͉�]��؂�
                    dashPow = 0; //�󒆂Ń_�b�V�����؂��Ɖ������c��ꍇ�����邽�߂����ł����Z�b�g���|����
                }
                

                if (Input.GetButton("Jump")) //�W�����v����
                {
                    //�W�����v�t���O���g�D���[
                    //��ւ̉������擾(�x�N�^�[3)
                    //���W�����v��͐ڒn���肪����邽�ߓ��͂����R�Ɛ؂��

                    if (!isJump)
                    {
                        SoundManager.instance.PlaySE(seJump); //���ʉ��Đ�(�d���������)
                    }

                    vVel = new Vector3(0, jumpSpeed, 0); //�W�����v����
                    isJump = true; //�W�����v������g�D���[

                    //�����ŃW�����v���̃����_����]�ʂ�����
                    jumpSpinVel.x = Random.Range(0, jumpSpin);
                    jumpSpinVel.y = Random.Range(0, jumpSpin);
                    jumpSpinVel.z = Random.Range(0, jumpSpin);
                    jumpSpinVel = jumpSpinVel.normalized;
                    jumpSpinVel.x *= 2f; //�c��]��傫�߂�

                    
                }
                else //�W�����v���Ȃ�
                {
                    if(!isDash) //�_�b�V�����łȂ�
                    {
                        MoveCtrl3(); //�ړ�����
                        vVel = Vector3.zero; //����OFF

                        //�_�b�V������
                        if (!dashAf && Input.GetKey(KeyCode.LeftShift)) //�ă_�b�V���\����Shift�L�[�����
                        {
                            isDash = true;
                            dashPow = dashSpeed; //�_�b�V�����x�␳������
                            dashSpinVel = Quaternion.Euler(0, 90, 0) * moveVel; //�i�s����moveVel��Y��90�x��]���_�b�V������]���Ƃ���
                            SoundManager.instance.PlaySE(seJump); //���ʉ��Đ�
                            Physics.IgnoreLayerCollision(layerP, layerE, true); //�Փ˔���𖳎�(���G���)
                        }
                    }
                    else //�_�b�V����
                    {
                        DashCtrl(); //�_�b�V������
                    }
                }
            }
            else
            {
                if (isKnockback) //�m�b�N�o�b�N��(�󒆂ŐH����Ă����삷��)
                {
                    KBackCtrl();
                    vVel = Vector3.zero; //����OFF
                }
                else if (!gCheck) //�󒆎��A�W�����v���܂�
                {
                    if(isJump) //�W�����v��
                    {
                        if (jumpTime >= 0.5f) //�W�����v�㏸���Ԃ͎b��0.5�b
                        {
                            jumpTime = 0; //�W�����v�㏸���ԃ��Z�b�g
                            isJump = false; //�����ɃV�t�g
                            isJumpDown = true;
                            isDash = false; //�����W�����v���̓_�b�V����������Z�b�g
                        }

                        vVel *= jumpCurve.Evaluate(jumpTime);
                        jumpTime += Time.deltaTime;
                    }
                    else //������
                    {
                        //���_�b�V�����͗����Ȃ�
                        if (!isDash)
                        {
                            vVel = new Vector3(0, -graSpeed, 0); //����
                        }
                        else
                        {
                            DashCtrl(); //�_�b�V������
                        }
                    }
                }
            }

            rb.velocity = (moveVel * ((runSpeed + dashPow) * jumpAccel)) + vVel; //�ŏI�I�Ȉړ����x����
            //������jumpAccel�͎g���ĂȂ�
        }
        else //���S���͏d�͋����̂�
        {
            if (isDeath) //���S������
            {
                if (deathTime >= 2f) //�����2�b�o������
                {
                    GManager.instance.pDeath = true; //G�}�l�[�W���[�Ɏ��S����𑗂�
                }

                vVel = new Vector3(0, -graSpeed, 0);

                deathTime += Time.deltaTime; //����ł���̎��ԃJ�E���g
            }
            else //���S���肪�o�Ă��Ȃ���HP��0�̎�
            {
                Death(); //���S����
            }

            rb.velocity = (moveVel * ((runSpeed + dashPow) * jumpAccel)) + vVel; //�ǉ��̈ړ��͂ł��Ȃ��������͎c��
        }
    }

    void Update()
    {
        if(!isDeath)
        {
            if (gCheck && !isDash)
            {

                DirCtrl3(); //�n��ł̌�������

                if (dashAf) //�_�b�V���㏈��
                {
                    if(!Input.GetKey(KeyCode.LeftShift)) //Shift�L�[�𗣂���
                    {
                        dashAf = false;
                    }
                }
            }
            else
            {
                if(isKnockback) //�m�b�N�o�b�N���͉�]���~�߂�
                {

                }
                else
                {
                    if (isJump || isJumpDown) //�W�����v����]
                    {
                        mu.transform.Rotate(jumpSpinVel * 720f * Time.deltaTime, Space.World);
                    }
                    else if (isDash) //�_�b�V������]
                    {
                        mu.transform.Rotate(dashSpinVel * 720f * Time.deltaTime, Space.World); //1�b��720�x��](0.5�b��1��])
                    }
                }
                
            }
                
        }
        else
        {
            
        }
    }


    /// <summary>
    /// �ړ�����v3
    /// </summary>
    private void MoveCtrl3()
    {
        //�������␳�Ȃ��̒P���ړ�
        float ad = Input.GetAxis("Horizontal"); //������
        float ws = Input.GetAxis("Vertical"); //�c����

        wsVel = new Vector3(0, 0, ws); //�O��ړ��l�̊�l
        adVel = new Vector3(ad, 0, 0); //���E�ړ��l�̊�l

        //������������C���J�����̕��ʓ��e���[�J���ɕϊ�
        Transform lookA = mainCam.transform; //�A���`
        lookA.localPosition = new Vector3(lookA.localPosition.x, this.transform.position.y, lookA.localPosition.z); //�A���v���C���[�Ɠ��������ɕύX
        Vector3 lookB = lookPosN0.transform.position; //�B���`
        lookB.y = this.transform.position.y; //�B���v���C���[�Ɠ��������ɕύX
        lookA.transform.LookAt(lookB); //�A���B�Ɍ�����(����������[�J���)
        moveVel = lookA.TransformDirection(wsVel + adVel).normalized; //�O��ړ��l�ƍ��E�ړ��l�����́A�����C���J�����̕��ʓ��e���[�J�������ɕϊ�(���m�[�}���C�Y)
    }

    /// <summary>
    /// �_�b�V������
    /// </summary>
    private void DashCtrl()
    {
        if (dashTime >= 0.5f) //�_�b�V�����Ԃ͎b��0.5�b
        {
            isDash = false; //�_�b�V�������胊�Z�b�g
            dashPow = 0f; //�_�b�V�����������Z�b�g
            dashTime = 0f; //�_�b�V�����ԃ��Z�b�g
            dashAf = true; //�_�b�V���㔻��J�n
            Physics.IgnoreLayerCollision(layerP, layerE, false); //�Փ˔���𕜊�
        }
        else
        {
            dashTime += Time.deltaTime; //�_�b�V�����ԍX�V
        }
    }

    /// <summary>
    /// ��������v3
    /// </summary>
    private void DirCtrl3()
    {
        Vector3 dir = GManager.instance.cursorPos.position - this.transform.position;
        dir.y = 0;

        mu.transform.rotation = Quaternion.LookRotation(dir, Vector3.up); //Y����]�ŃJ�[�\������������
    }

    /// <summary>
    /// ��e������
    /// </summary>
    private void KBackCtrl()
    {
        kbTime += Time.deltaTime; //�o�ߎ��ԉ��Z

        if (kbTime <= 0.5f) //�J�E���g0.5�b�܂�
        {
            if(!isDamage)
            {
                isDamage = true; //�_���[�W������ǉ��ōs��Ȃ��悤�ɂ���(�m�b�N�o�b�N��ɉ���)
                HPDown(); //HP��������

                if (GManager.instance.hpNow <= 0) //HP��0�����������
                {
                    Death(); //���S����
                }
            }
        }
        else //0.5�b�o��
        {
            kbTime = 0f; //�J�E���g���Z�b�g
            isKnockback = false; //�m�b�N�o�b�N���胊�Z�b�g
            isDamage = false; //�_���[�W���胊�Z�b�g
        }
            
        //�m�b�N�o�b�N���͑��̓G�ɐڐG���Ă��ǉ��_���[�W���󂯂Ȃ�
        //������������̑��Ε������擾���A���Α��Ƀm�b�N�o�b�N����悤�ɂ���
    }

    /// <summary>
    /// HP��������
    /// </summary>
    private void HPDown()
    {
        GManager.instance.PHPDown(); //HP��������
        GManager.instance.isDam = false; //G�}�l�[�W���[�̔�e��������������Z�b�g
    }


    /// <summary>
    /// ���S����
    /// </summary>
    private void Death()
    {
        isDeath = true; //���S������g�D���[��
        SoundManager.instance.PlaySE(seDead);
        Instantiate(efDead, this.transform.position, Quaternion.identity, this.transform); //���S�������Ԃ��G�t�F�N�g����
        rb.constraints = RigidbodyConstraints.None; //�I�u�W�F�N�g�̉�]�����𖳌�(�|���)
    }

    /// <summary>
    /// ��e������
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(!isKnockback) //�m�b�N�o�b�N���łȂ���
        {
            if (collision.gameObject.tag == "Enemy")
            {
                isKnockback = true; //�m�b�N�o�b�N������g�D���[
                hitEnemy = collision.gameObject; //�Ԃ����������ۑ�
                moveVel = transform.position - hitEnemy.transform.position; //�m�b�N�o�b�N����������
                moveVel = new Vector3(moveVel.x, 0, moveVel.z).normalized; //�m�b�N�o�b�N�����𐅕������Ńm�[�}���C�Y
                SoundManager.instance.PlaySE(seDamage);
            }
        }
        
    }
}
