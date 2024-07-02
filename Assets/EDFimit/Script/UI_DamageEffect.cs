using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ダメージを受けた際の画面が赤くなる効果
//プレハブをGマネージャー経由でInstantiateで呼び出すと一回明滅(アルファ値1で出現し透明化？)しデストロイ

public class UI_DamageEffect : MonoBehaviour
{
    [Header("左側")] public Image efL;
    [Header("右側")] public Image efR;

    private float al = 1f; //アルファ値

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(al <= 0) //透明化完了
        {
            Destroy(this);
        }
        else
        {
            efL.color = new Color(1, 0, 0, al); //アルファ値を更新
            efR.color = new Color(1, 0, 0, al); //アルファ値を更新

            al -= Time.deltaTime; //経過時間分減算
        }
    }
}
