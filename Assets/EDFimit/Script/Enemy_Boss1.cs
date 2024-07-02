using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Boss1 : MonoBehaviour
{
    [Header("�d�͒l")] public float graSpeed;
    [Header("�ڒn����")] public GroundCheck gc;
    [Header("���G����")] public SightCheck sc;
    [Header("��eSE")] public AudioClip seDamage;
    [Header("���SSE")] public AudioClip seDead;
    [Header("�����Ԃ�")] public GameObject bloodEf;

    private EnemyParameter ep;
    private Rigidbody rb; //���W�b�h�{�f�B�擾�p
    private NavMeshAgent agent; //���̓G�̃i�r���b�V���G�[�W�F���g
    private Transform target; //�i�r���b�V������̈ړ���
    private bool gCheck; //�ڒn����
    private bool sCheck; //���G����
    private bool modeA = false; //��ԃt���O�F�U��
    private bool deadShift; //���S�����񋓓�
    private bool isTarg; //�^�[�Q�b�g�擾��ԃt���O
    private float timeCount; //�O�i/���S���ԑ���p
    private float nextTime; //���̃^�[�Q�b�g�Ď擾�܂ł̎���(�����_���ݒ肷��)
    private Vector3 hitPos; //�e�ۂ̐ڐG���W


    void Start()
    {
        ep = GetComponent<EnemyParameter>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        target = GManager.instance.player.transform;

        rb.isKinematic = true;
        modeA = true; //�U�����[�h��
    }


    void Update()
    {
        if (!ep._death)
        {
            gCheck = gc.IsGround(); //�ڒn����
            sCheck = sc.IsSight(); //���G����

            if (gCheck) //�ڒn���蒆
            {
                if(!sCheck)
                {
                    if (modeA)
                    {
                        MoveA();
                    }
                    else
                    {
                        //�����Ȃ�(��~���[�h)
                    }
                }
                else
                {
                    ModeD();
                }
            }
            else //�󒆋���
            {
                transform.position -= transform.up; //���ɓ�����(������)
            }

        }
        else
        {
            if (!deadShift)
            {
                deadShift = true; //���S�����񋓓����I��
                rb.isKinematic = false; //�������Z����
                agent.enabled = false; //�i�r���b�V���G�[�W�F���g�𖳌�
                timeCount = 0; //�J�E���g���Z�b�g(timeCount�ϐ��͈ړ����Ǝ��S���ŋ��L���Ă��邽��)
                rb.constraints = RigidbodyConstraints.None; //�I�u�W�F�N�g�̉�]�����𖳌�(�|���)
                SoundManager.instance.PlaySE(seDead);
            }
            else
            {
                Dead(); //���S������
                rb.velocity = new Vector3(0, -graSpeed, 0); //�d�͋����̂�
            }
        }

    }


    /// <summary>
    /// ���[�hA����
    /// </summary>
    private void MoveA()
    {
        if (!isTarg)
        {
            //�^�[�Q�b�g�擾���[�h
            target = GManager.instance.pTracer.transform; //�^�[�Q�b�g�w��
            agent.SetDestination(target.position); //�^�[�Q�b�g���W���擾
            nextTime = Random.Range(0.3f, 1.0f); //���̍Ď擾���Ԑݒ�
            isTarg = true;

            //���{�X�͋����ɂ�鎩�����łȂ�
        }
        else
        {
            //�^�[�Q�b�g�ւ̈ړ����[�h
            if (timeCount >= nextTime) //�Ď擾���ԕ��ړ�����
            {
                isTarg = false; //�^�[�Q�b�g�Ď擾��
                timeCount = 0; //�J�E���g���Z�b�g

                if (GManager.instance.pDeath) //�v���C���[�̎��S����
                {
                    modeA = false; //�U�����[�h�����Z�b�g
                    agent.ResetPath(); //�o�H���Z�b�g
                }
            }

            timeCount += Time.deltaTime; //�o�ߎ��ԉ��Z
        }
    }

    /// <summary>
    /// ���[�hD����
    /// </summary>
    private void ModeD()
    {
        //�U���̃C���^�[�o���Ȃ�
        target = GManager.instance.pTracer.transform; //�^�[�Q�b�g�w��
        agent.SetDestination(target.position); //�^�[�Q�b�g���W���擾
    }

    /// <summary>
    /// ���S������
    /// </summary>
    private void Dead()
    {
        if (timeCount <= 5.0f) //�J�E���g5�b�܂�
        {
            timeCount += Time.deltaTime; //�o�ߎ��ԉ��Z
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// �ڐG��
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet") //��e��
        {
            hitPos = collision.contacts[0].point; //�ڐG���W���擾
            Instantiate(bloodEf, this.transform.position, Quaternion.LookRotation(hitPos), this.transform); //�����Ԃ�
            SoundManager.instance.PlaySE(seDamage); //��eSE�Đ�
        }
        else if (collision.gameObject.tag == "Player") //�U����
        {
            GManager.instance.EAtkCange(1f); //�����I�ɂ̓G�l�~�[�p�����[�^���ŏ����H
        }

    }
}
