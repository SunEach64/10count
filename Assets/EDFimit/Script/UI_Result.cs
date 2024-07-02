using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Result : MonoBehaviour
{
    [Header("スコア")] public TMP_Text txScore;
    [Header("撃破数")] public TMP_Text txCount;
    [Header("個別撃破数")] public string[] txInd; //個別の撃破表記

    private int loop; //ループ回数
    private int countQty; //txCountの項目数
    private int zombieCount;
    private int bossZCount;
    private float timer;
    private bool afRslt; //リザルト完了フラグ
    private bool rslt0; //スコア表示
    private bool rslt1; //カウント表示
    private bool reset; //リセットボタン表示へ

    void Start()
    {
        txCount.text = null; //一度空にする
        ECount(); //各撃破数を取得
        rslt0 = true;
    }

    void Update()
    {
        //撃破数カウントの表示は0.5秒きざみで表示
        //全体スコア表示→0.5秒→ゾンビ表示→0.5秒→ボスゾンビ表示→0.5秒→…
        //撃破した敵種類に応じてループで行数を決定？

        if(!afRslt)
        {
            if (rslt0)
            {
                txScore.text = "Result\nSCORE : " + GManager.instance.score.ToString("f0"); //スコア表示
                rslt0 = false;
                rslt1 = true;
                timer = 0; //タイマーリセット
            }
            if (rslt1 && timer >= 0.5f) //0.5秒刻みで敵撃破数を表示していく
            {
                txCount.text = txCount.text + "\n" + txInd[loop] ; //撃破数を次の行に追加

                if (loop < countQty) //ループ継続
                {
                    loop++;
                    timer = 0; //都度タイマーをリセット
                }
                else //ループ抜けてリセットボタン表示へ
                {
                    rslt1 = false;
                    reset = true;
                    timer = 0;
                }

            }
            if (reset && timer >= 0.5f) //リセットボタン表示
            {
                GManager.instance.uic.ResetActive(); //リセットボタン表示
                reset = false;
                afRslt = true;
            }

            timer += Time.unscaledDeltaTime;
        }
    }

    private void ECount()
    {
        //対応する敵番号はEnemyCountスクリプト参照
        //1体以上倒していた場合、countQtyが1増えてその番号のテキストに割り振られる
        //一通り集計後、countQtyが倒した敵種類数になるため、それをループの実行回数にする？

        zombieCount = GManager.instance.ec.eCount[0]; //ゾンビの撃破数を取得
        if(zombieCount >= 0)
        {
            //とりあえず通常ゾンビのリザルト表記は撃破0体でも表示させる
            txInd[0] = "Zombie : " + zombieCount;
        }
        bossZCount = GManager.instance.ec.eCount[1]; //ボスゾンビの撃破数を取得
        if(bossZCount > 0)
        {
            countQty += 1;
            txInd[countQty] = "BossZombie : " + bossZCount;
        }
    }
}
