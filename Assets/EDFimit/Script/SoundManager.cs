using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] AudioSource aSouce;
    [SerializeField] AudioClip testSE; //�{�����[���ύX���̃e�X�g��
    [HideInInspector] public float bfVol = 0.5f; //�O�̃{�����[��
    [HideInInspector] public float firstVol = 0.5f; //��������
    [HideInInspector] public Slider slider;

    [HideInInspector] public bool startSet; //�Q�[���J�n���̉���0�s����]�����߂̏����p

    private bool isChangeVolume; //���ʕύX���s�����ۂ̃t���O

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        
        aSouce.volume = 0.5f; //��������
    }

    void Update()
    {
        //�}�E�X�{�^���𗣂����A���{�����[�����ω�
        if (Input.GetMouseButtonUp(0) && isChangeVolume)
        {
            PlaySE(testSE); //�e�X�g���炷
            bfVol = aSouce.volume; //�O�{�����[���X�V
            isChangeVolume = false; //�t���O���Z�b�g
        }
        
    }

    /// <summary>
    /// �{�����[���̐ݒ�
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeVolume(float volume)
    {
        //UI_SoundVolume����Ăяo����A�X���C�_�[�l�Ń{�����[���ύX
        aSouce.volume = volume;

        if(!startSet)
        {
            //�J�n�����ʒ������I���܂ŉ������Ȃ�
        }
        else
        {
            isChangeVolume = true; //���ʕύX������g�D���[
        }
    }

    public void PlaySE(AudioClip clip)
    {
        aSouce.PlayOneShot(clip);
    }

}
