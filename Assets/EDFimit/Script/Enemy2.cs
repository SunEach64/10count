using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour
{
    [Header("�d�͒l")] public float graSpeed;
    [Header("���ŋ���")] public float desDis;
    [Header("�ڒn����")] public GroundCheck gc;
    [Header("���SSE")] public AudioClip seDead;
    [Header("�����Ԃ�")] public GameObject bloodEf;

    private EnemyParameter ep;
    private Rigidbody rb; //���W�b�h�{�f�B�擾�p
    private NavMeshAgent agent; //���̓G�̃i�r���b�V���G�[�W�F���g
    private Transform target; //�i�r���b�V������̈ړ���
    private bool gCheck; //�ڒn����
    private bool modeA = false; //��ԃt���O�F�U��
    private bool deadShift; //���S�����񋓓�
    private bool isTarg; //�^�[�Q�b�g�擾��ԃt���O
    private float timeCount; //�O�i/���S���ԑ���p
    private float nextTime; //���̃^�[�Q�b�g�Ď擾�܂ł̎���(�����_���ݒ肷��)
    private Vector3 hitPos; //�e�ۂ̐ڐG���W


    //�E���񃂁[�h
    //�����_���ɓ���������
    //��e��v���C���[���F�ɂ���čU�����[�h�ֈȍ~
    //�E�U�����[�h
    //5�b�ԃv���C���[�̈ʒu�𖳏����Ŕc�����A�����ֈړ�����(�ڐG�ɂ��v���C���[�̓_���[�W���󂯂�)
    //5�b�o�ߎ��ɍ��G�͈͂Ƀv���C���[�����Ȃ���΁A���̏�ŁH�b�ԑҋ@�A���̌㏄��or�ҋ@���[�h�Ɉȍ~


    void Start()
    {
        ep = GetComponent<EnemyParameter>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        target = GManager.instance.player.transform;

        rb.isKinematic = true;
        modeA = true; //�U�����[�h��
    }

    void FixedUpdate()
    {
        if(!ep._knockBack)
        {
            //�t�H���X�̎��͉����Ȃ�
        }
        else
        {
            KnockBack(); //�m�b�N�o�b�N���\�b�h

            ep._knockBack = false; //�d�������Ȃ����߃��Z�b�g
        }
    }

    void Update()
    {
        if (!ep._death)
        {
            gCheck = gc.IsGround(); //�ڒn����

            if (gCheck) //�ڒn���蒆
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
            else //�󒆋���
            {
                transform.position -= transform.up; //���ɓ�����(������)
            }

        }
        else
        {
            if(!deadShift)
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
            nextTime = Random.Range(0.1f, 0.5f); //���̍Ď擾���Ԑݒ�
            isTarg = true;

            //�v���C���[�Ƃ̋����𔻒�
            if(Vector3.Distance(this.transform.position, target.position) > desDis)
            {
                Debug.Log("��������");
                Destroy(this.gameObject);
            }
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
    /// �m�b�N�o�b�N����
    /// </summary>
    private void KnockBack()
    {
        //�m�b�N�o�b�N�̃R���[�`�����J�n
        StartCoroutine(KnockbackRoutine(ep.kbDir.normalized, ep.kbForce, ep.kbTime));
    }

    /// <summary>
    /// �m�b�N�o�b�N�R���[�`��
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="force"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator KnockbackRoutine(Vector3 direction, float force, float duration)
    {
        // NavMeshAgent���ꎞ�I�ɖ�����
        agent.enabled = false;

        // Rigidbody��Kinematic�������
        rb.isKinematic = false;

        // �m�b�N�o�b�N�̗͂�������
        rb.AddForce(direction * force, ForceMode.Impulse);

        // �w�肳�ꂽ���ԃm�b�N�o�b�N�𑱂���
        yield return new WaitForSeconds(duration);

        if(!ep._death) //�m�b�N�o�b�N�I�����Ɏ��S���Ă��Ȃ����
        {
            // Rigidbody���Ă�Kinematic�ɐݒ�
            rb.isKinematic = true;

            // NavMeshAgent���ēx�L����
            agent.enabled = true;
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
            Instantiate(bloodEf, this.transform.position, Quaternion.LookRotation(hitPos), this.transform); //�����Ԃ�����
        }
        else if (collision.gameObject.tag == "Player") //�U����
        {
            GManager.instance.EAtkCange(1f); //�����I�ɂ̓G�l�~�[�p�����[�^���ŏ����H
        }

    }
}
