using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//マウスカーソルの位置に照準を表示させる
//及びレイキャストでマウスポイント位置に照準が向くようにする

public class CursorCtrl : MonoBehaviour
{
    [Header("カーソル画像位置")] public RectTransform cPos;
    [Header("補正有り照準")] public GameObject aimPos;

    [HideInInspector] public Vector3 cursorPos; //カーソル座標(プレイヤー高さ)

    private Plane plane = new Plane(); //Planeを定義：CursorPoint用
    private float distance = 0; //距離：CursorPoint用
    private bool aimSupport; //エイム補正フラグ


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!GManager.instance.isGOver) //ゲームオーバー後、リセット前にここの処理を止めないとエラーが出る
        {
            if(GManager.instance.gc.IsGround()) //接地中
            {
                plane.SetNormalAndPosition(Vector3.up, GManager.instance.player.transform.position); //プレイヤー高さ
            }
            else //空中
            {
                plane.SetNormalAndPosition(Vector3.up, GManager.instance.pTracer.transform.position); //Pトレーサー高さ
            }

            CursorPoint(); //cursorPosを毎フレーム更新
            transform.position = cursorPos; //照準位置をcursorPosに移動(ShotCtrl側で高さを再度補正する)
        }
        
        cPos.position = Input.mousePosition; //照準画像を移動
        
    }

    /// <summary>
    /// プレイヤー高さカーソル位置
    /// </summary>
    void CursorPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //メインカメラからマウスカーソルの座標へRayを飛ばす

        if (plane.Raycast(ray, out distance))  //planeとRayが交差しているかを判定、且つtrueの時は更に原点(カメラ)から交差点までの距離を取得しdistanceへ入れる
        {
            cursorPos = ray.GetPoint(distance); //カーソルの座標を定義(Rayのdistance距離座標、つまりPlaneとRayの交差座標、つまりマウスカーソルの位置)
        }
        else
        {

        }

        //※Plane.Raycast()：RayがPlaneと交差する時trueを返す
        //Physics.Raycast()と違いPlaneを対象にすることから建物など障害物を貫通して位置を取得できる
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            aimPos.transform.position = other.transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy")
        {
            aimPos.transform.position = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            aimPos.transform.position = this.transform.position;
        }
    }
}
