using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SoundVolume : MonoBehaviour
{
    private Slider sv; //�X���C�_�[
    private float startSV; //�J�n���X���C�_�[�l 

    void Start()
    {
        sv = GetComponent<Slider>();
        startSV = SoundManager.instance.bfVol;
        sv.value = startSV;
    }

    void Update()
    {
        if(!SoundManager.instance.startSet) //����J�n��(���g���C���͖��������)
        {
            //�J�n�����ʂɃX���C�_�[�𒲐�
            if(sv.value == SoundManager.instance.firstVol)
            {
                SoundManager.instance.startSet = true; //����ݒ苓�����I��
            }
            else
            {
                sv.value = SoundManager.instance.firstVol;
            }
        }
    }

    public void SetVolume(float volume)
    {
        SoundManager.instance.ChangeVolume(volume);
    }
}
