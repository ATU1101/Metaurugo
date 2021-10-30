using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public LayerMask StageLayer;

    // 動作状態を定義する 
    private enum MOVE_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
    }
    MOVE_TYPE move = MOVE_TYPE.STOP; // 初期状態はSTOP 
    Rigidbody2D rbody2D;             // Rigidbody2Dを定義
    float speed;                     // 移動速度を格納する変数

    private void Start()
    {
        // Rigidbody2Dのコンポーネントを取得
        rbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizonkey = Input.GetAxis("Horizontal");      // 水平方向のキー取得

        // 取得した水平方向のキーを元に分岐
        if (horizonkey == 0)
        {
            // キー入力なしの場合は停止
            move = MOVE_TYPE.STOP;
        }
        else if (horizonkey > 0)
        {
            // キー入力が正の場合は右へ移動する
            move = MOVE_TYPE.RIGHT;
        }
        else if (horizonkey < 0)
        {
            // キー入力が負の場合は左へ移動する
            move = MOVE_TYPE.LEFT;
        }

        // spaceを押したらジャンプ関数へ
        if (GroundChk())
        {
            if (Input.GetKeyDown("space"))
            {
                Jump();
            }
        }
    }

    // 物理演算(rigidbody)はFixedUpdateで処理する
    private void FixedUpdate()
    {
        // Playerの方向を決めるためにスケールの取り出し
        Vector3 scale = transform.localScale;
        if (move == MOVE_TYPE.STOP)
        {
            speed = 0;
        }
        else if (move == MOVE_TYPE.RIGHT)
        {
            scale.x = 1; // 右向き
            speed = 3;
        }
        else if (move == MOVE_TYPE.LEFT)
        {
            scale.x = -1; // 左向き
            speed = -3;
        }
        transform.localScale = scale; // scaleを代入
        // rigidbody2Dのvelocity(速度)へ取得したspeedを入れる。y方向は動かないのでそのままにする
        rbody2D.velocity = new Vector2(speed, rbody2D.velocity.y);
    }

    void Jump()
    {
        // 上に力を加える
        rbody2D.AddForce(Vector2.up * 360);
    }

    bool GroundChk()
    {
        Vector3 startposition = transform.position;                     // Playerの中心を始点とする
        Vector3 endposition = transform.position - transform.up * 0.53f; // Playerの足元を終点とする

        // Debug用に始点と終点を表示する
        Debug.DrawLine(startposition, endposition, Color.red);

        // Physics2D.Linecastを使い、ベクトルとStageLayerが接触していたらTrueを返す
        return Physics2D.Linecast(startposition, endposition, StageLayer);
    }
}