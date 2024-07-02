using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//HPドット個々の挙動
//主にダメージ時の消滅演出

public class UI_HP_Dot : MonoBehaviour
{
    [HideInInspector] public bool isVanish; //ドットの消滅フラグ、UI_HPから制御

    private float al = 1f; //アルファ値
    private Image im;

    // Start is called before the first frame update
    void Start()
    {
        im = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isVanish)
        {
            if(al <= 0f)
            {
                al = 0f;
                isVanish = false; //消滅フラグをオフにして以降の無駄な処理を省く
            }
            else
            {
                al -= Time.deltaTime; //経過時間を減算
                im.color = new Color(0, 0, 0.78f, al); //ドットを透明にしていく
            }
        }
    }
}
