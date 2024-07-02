using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private string groundTag = "Ground"; //地面タグ
    private bool isGround = false; //地面との接触判定フラグ
    private bool isGroundEnter, isGroundStay, isGroundExit; //ｵﾝﾄﾘｶﾞｰ3種それぞれの判定フラグ

    /// <summary>
    /// 地面との接触判定
    /// </summary>
    /// <returns></returns>
    public bool IsGround()
    {
        if (isGroundEnter || isGroundStay) //接触したか、接触し続けている場合
        {
            isGround = true; //接地判定トゥルー
        }
        else if (isGroundExit) //接触を離れた場合
        {
            isGround = false; //接地判定ファルス
        }

        //3種フラグのリセット
        isGroundEnter = false; //接触時フラグ
        isGroundStay = false; //接触中フラグ
        isGroundExit = false; //接触解除フラグ

        return isGround; //決定した接触判定を返す
    }

    /// <summary>
    /// 地面と接触時
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == groundTag) //GroundCheckオブジェクトにGroundタグのオブジェクトが接触した
        {
            isGroundEnter = true; //接触時フラグをトゥルー
        }
    }

    /// <summary>
    /// 地面と接触し続けている
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == groundTag) //GroundCheckオブジェクトにGroundタグのオブジェクトが接触し続けている
        {
            isGroundStay = true; //接触中フラグをトゥルー
        }
    }

    /// <summary>
    /// 地面から離れた
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == groundTag) //GroundCheckオブジェクトからGroundタグのオブジェクトが離れた
        {
            isGroundExit = true; //接触解除フラグをトゥルー
        }
    }

}
