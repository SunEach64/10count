using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HP : MonoBehaviour
{
    [Header("アンダーバー")] public Image ub;
    [Header("UI_HP_Dot")] public UI_HP_Dot[] uihpd;
    [Header("HPドット")] public Image[] hpD;
    [Header("HPグロウ")] public Image[] hpG;
    [Header("HPテキスト")] public TMP_Text hpText;

    private bool isStart; //スタート演出中
    private bool isPlay; //プレイ中
    private bool isDamage; //ダメージ処理中
    private int dn; //HPドットの番号

    void Start()
    {
        isStart = true; //アクティブになると同時にスタート演出開始
        ub.fillAmount = 0f; //アンダーバーのフィルアマウントを0から開始
    }

    void Update()
    {
        if(isPlay) //プレイ中
        {

        }
        else if(isStart) //スタート演出中
        {
            if (ub.fillAmount >= 1f) //アンダーバーが1まで伸びる
            {
                ub.fillAmount = 1f; //長さを1に確定
                hpText.color = new Color(0, 0, 1, ub.fillAmount);
                hpD[0].color = new Color(0, 1, 0.78f, ub.fillAmount);
                hpD[1].color = new Color(0, 1, 0.78f, ub.fillAmount);
                hpD[2].color = new Color(0, 1, 0.78f, ub.fillAmount);
                hpG[0].color = new Color(1, 1, 1, ub.fillAmount);
                hpG[1].color = new Color(1, 1, 1, ub.fillAmount);
                hpG[2].color = new Color(1, 1, 1, ub.fillAmount);
                isPlay = true; //プレイ中処理に移行
            }
            else
            {
                ub.fillAmount += Time.deltaTime; //アンダーバーを伸ばす

                //アンダーバー上のテキストやイメージのアルファ値をアンダーバーのフィルアマウントに連動させて上げる
                hpText.color = new Color(0, 0, 1, ub.fillAmount);
                hpD[0].color = new Color(0, 1, 0.78f, ub.fillAmount);
                hpD[1].color = new Color(0, 1, 0.78f, ub.fillAmount);
                hpD[2].color = new Color(0, 1, 0.78f, ub.fillAmount);
                hpG[0].color = new Color(1, 1, 1, ub.fillAmount);
                hpG[1].color = new Color(1, 1, 1, ub.fillAmount);
                hpG[2].color = new Color(1, 1, 1, ub.fillAmount);
            }
             
        }

        //※開始時
        //スタートボタンに連動(Gマネージャーによりアクティブ化)
        //アンダーバーがフィルアマウントで左から右に伸びる
        //伸びたら、もしくは同時に、HPと●が透明から不透明に遷移

        //※プレイ中
        //ダメージ時に右から順に●が消える
        //内部では数値として管理している
        //●が消える時は色が変わる(●は黒っぽく、周りのglow部分は赤に？)
        //色変化後に透明化で消える
        //●とグロウは配列でアクセスし、残りHP値に関連付けて透明化する(完全に非表示化の方が良い？)
    }

    public void HPDown()
    {
        dn = (int)GManager.instance.hpNow; //hpNowはfloat型なのでint型に変換
        uihpd[dn].isVanish = true; //対応HPドットの消滅フラグをトゥルーに
                                   //一回目の被弾時はhpNowが2になり、hpC[2]で3つ目のHP球にアクセス
    }

    /// <summary>
    /// HP強制0
    /// </summary>
    public void HPDownAll()
    {
        uihpd[0].isVanish = true;
        uihpd[1].isVanish = true;
        uihpd[2].isVanish = true;
    }
}
