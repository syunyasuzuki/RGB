using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ctr : MonoBehaviour
{
    float player_scale = 0;

    public void Setplayer_scale(float scale)
    {
        player_scale = scale;
    }


    Rigidbody2D rg2D;

    Animator anima;

    float speed_x;
    float speed = 1000;
    float jump;
    float jump_Force = 500;
    float max_speed = 4.0f;

    float move_x;
    Vector2 move;

    bool move_check;

    // Start is called before the first frame update
    void Start()
    {
        move_check = true;

        rg2D = GetComponent<Rigidbody2D>();

        anima = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (move_check == true)
        {
            Move();
            Player_Animation();
        }
    }

    void Move()
    {
        // 左右移動の処理
        if (Input.GetKey(KeyCode.A))
        {
            move_x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            move_x = 1;
        }
        else
        {
            move_x = 0;
        }

        move = new Vector2(move_x * speed * Time.deltaTime, 0.0f);

        speed_x = Mathf.Abs(rg2D.velocity.x);

        if (speed_x < max_speed)
        {
            rg2D.AddForce(move);
        }

        // ジャンプの処理
        jump = Mathf.Abs(rg2D.velocity.y);

        if (Input.GetKeyDown(KeyCode.W) && rg2D.velocity.y == 0)
        {
            rg2D.AddForce(transform.up * jump_Force);
        }
    }

    void Player_Animation()
    {
        if (jump > 0.01f)
        {
            anima.SetFloat("JumpFloat", jump);
        }
        else
        {
            anima.SetFloat("JumpFloat", 0.0f);
        }

        if (speed_x > 0.1f)
        {
            anima.SetFloat("WalkFloat", speed_x);
        }
        else
        {
            anima.SetFloat("WalkFloat", 0.0f);
        }

        //プレイヤーの向きを変える
        if (move_x != 0)
        {
            transform.localScale = new Vector3(move_x * player_scale, player_scale, 1);
        }
    }
}
