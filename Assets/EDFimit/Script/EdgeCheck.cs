using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCheck : MonoBehaviour
{
    private string cTag = "Edge"; //対象タグ
    private bool isCheck = false; //対象との接触判定フラグ
    private bool isCheckEnter, isCheckStay, isCheckExit; //ｵﾝﾄﾘｶﾞｰ3種それぞれの判定フラグ

    /// <summary>
    /// 接触判定
    /// </summary>
    /// <returns></returns>
    public bool IsEdge()
    {
        if (isCheckEnter || isCheckStay) //接触したか、接触し続けている場合
        {
            isCheck = true; //接地判定トゥルー
        }
        else if (isCheckExit) //接触を離れた場合
        {
            isCheck = false; //接地判定ファルス
        }

        //3種フラグのリセット
        isCheckEnter = false; //接触時フラグ
        isCheckStay = false; //接触中フラグ
        isCheckExit = false; //接触解除フラグ

        return isCheck; //決定した接触判定を返す
    }

    /// <summary>
    /// 対象と接触時
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == cTag)
        {
            isCheckEnter = true; //接触時フラグをトゥルー
        }
    }

    /// <summary>
    /// 対象と接触し続けている
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == cTag)
        {
            isCheckStay = true; //接触中フラグをトゥルー
        }
    }

    /// <summary>
    /// 対象から離れた
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == cTag)
        {
            isCheckExit = true; //接触解除フラグをトゥルー
        }
    }
}
