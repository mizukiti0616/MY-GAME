using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{

    Rigidbody rb;

    public float sphereRadius;


    //移動スピード
    public float speed = 2f;
    //ジャンプ力
    public float thrust = 100;
    //Animatorを入れる変数
    private Animator animator;
    //Planeに触れているか判定するため
    bool ground;

    public GameObject coinPrefab;


    private float posRange = 3.4f;

    private float pos = 0.69f;

    float offset = 2.2f;
    private GameObject ScoreText;

    private int score = 0;


    private GameObject stateText;


    private bool isEnd = false;

    public bool  attacked;

    //左ボタン押下の判定（追加）
    private bool UpButton = false;
    //右ボタン押下の判定（追加）
    private bool DownButton = false;
    private bool JumpButton = false;



    void Start()
    {
        this.ScoreText = GameObject.Find("ScoreText");

        rb = GetComponent<Rigidbody>();
        //UnityちゃんのAnimatorにアクセスする
        animator = GetComponent<Animator>();

        this.stateText = GameObject.Find("GameResultText");
    }

    void Update()
    {



        //地面に触れている場合発動
        if (ground)
        {
            //上下左右のキーでの移動、向き、アニメーション
            if ((Input.GetKey(KeyCode.S )|| this.DownButton))
            {
                //移動(X軸、Y軸、Z軸）
                rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);
                //向き(X軸、Y軸、Z軸）
                transform.rotation = Quaternion.Euler(0, 90, 0);
                //アニメーション
                animator.SetBool("Running", true);
            }
            else if ((Input.GetKey(KeyCode.W) || this.UpButton))
            {
                rb.velocity = new Vector3(-speed, rb.velocity.y, rb.velocity.z);
                transform.rotation = Quaternion.Euler(0, 270, 0);
                animator.SetBool("Running", true);
            }
            else if (Input.GetKey(KeyCode.RightArrow))

            {
                rb.velocity = new Vector3(rb.velocity.y, rb.velocity.z, speed);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                animator.SetBool("Running", true);
            }

            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.velocity = new Vector3(rb.velocity.y, rb.velocity.z, -speed);
                transform.rotation = Quaternion.Euler(0, 180, 0);
                animator.SetBool("Running", true);
            }
            //何もキーを押していない時はアニメーションをオフにする
            else
            {
                animator.SetBool("Running", false);
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);

            }

            //スペースキーでジャンプする
            if ((Input.GetKeyDown(KeyCode.Space) || this.JumpButton))
            {
                animator.SetBool("Jumping", true);
                //上方向に向けて力を加える
                rb.AddForce(new Vector3(0, thrust, 0));
                ground = false;
                JumpButton = false;
            }
            else
            {
                animator.SetBool("Jumping", false);

            }
        }
    }

    //別のCollider、今回はPlaneに触れているかどうかを判断する
    void OnCollisionStay(Collision col)
    {
        ground = true;
    }

   


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "block")
        {
            var block = collision.gameObject.GetComponent<Block>();
            if (block != null)
            {
                if (!block.attacked)
                {
                    block.attacked = true;
                    Vector3 blockPos = collision.gameObject.transform.position;
                    GameObject coin = Instantiate(coinPrefab) as GameObject;
                    coin.transform.position = new Vector3(blockPos.x, blockPos.y + offset, blockPos.z);

                }
            }

           

            }


        if (collision.collider.gameObject.tag == "CoinTag")
        {

            // スコアを加算(追加)
            this.score += 100;

            //ScoreText獲得した点数を表示(追加)
            this.ScoreText.GetComponent<Text>().text = "Score " + this.score + "pt";

        }

        //ゴール地点に到達した場合
        if (collision.collider.gameObject.tag == "Finish")
        {
            this.isEnd = true;
            //stateTextにGAME CLEARを表示（追加）
            this.stateText.GetComponent<Text>().text = "CLEAR!!";
        }

     











    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GameOver")
        {
            this.isEnd = true;

            this.stateText.GetComponent<Text>().text = "GAME OVER";

        }









    }

    public void GetMyUpButtonUp()
    {
        this.UpButton = true;
    }

    public void GetMyDownButtonUp()
    {
        this.UpButton = false;
    }

    public void GetMyUpButtonDown()
    {
        this.DownButton = true;
    }
   
    public void GetMyDownButtonDown()
    {
        this.DownButton = false;
    }
    public void GetMyJumpButtonUp()
    {
        this.JumpButton = true;
    }

    public void GetMyJumpButtonDown()
    {
        this.JumpButton = false;
    }












}



