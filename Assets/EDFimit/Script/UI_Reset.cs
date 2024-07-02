using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Reset : MonoBehaviour
{
    public void OnPressed()
    {
        SoundManager.instance.firstVol = SoundManager.instance.bfVol; //���Z�b�g���̏��񉹗ʂ�ݒ�
        SoundManager.instance.startSet = false; //�T�E���h�}�l�[�W���[�̏��񋓓��t���O�����Z�b�g

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
