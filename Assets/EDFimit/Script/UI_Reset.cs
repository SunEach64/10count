using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Reset : MonoBehaviour
{
    public void OnPressed()
    {
        SoundManager.instance.firstVol = SoundManager.instance.bfVol; //リセット時の初回音量を設定
        SoundManager.instance.startSet = false; //サウンドマネージャーの初回挙動フラグをリセット

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
