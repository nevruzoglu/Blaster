using System;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region  Singleton


    private static Paddle _instance;
    public static Paddle Instance => _instance;


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion


    private Camera mainCamera;
    private float paddleInitialY;
    private float defaultPaddleWitdthPixels = 200;
    private float defaultLeftClamp = 145;
    private float defaultRightClamp = 500;
    private SpriteRenderer sr;



    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        paddleInitialY = this.transform.position.y;
        sr = GetComponent<SpriteRenderer>();


    }


    void Update()
    {
        PaddleMovement();
    }

    private void PaddleMovement() // Platform sağ sol sınırı, büyüdüğünde sınır sabit kalıyor
    {
        float paddleShift = (defaultPaddleWitdthPixels - ((defaultPaddleWitdthPixels / 2) * this.sr.size.x)) / 2;
        float leftClamp = defaultLeftClamp - paddleShift;
        float righClamp = defaultRightClamp + paddleShift;
        float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, righClamp);
        float mousePositionWorldX = mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels, 0, 0)).x;
        this.transform.position = new Vector3(mousePositionWorldX, paddleInitialY, 0);
    }


    private void OnCollisionEnter2D(Collision2D coll) // topun platforma çarma yerine göre sağa sola gitmesi
    {
        if (coll.gameObject.tag == "Ball")
        {

            Rigidbody2D ballrb = coll.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitpoint = coll.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            ballrb.velocity = Vector2.zero;

            float difference = paddleCenter.x - hitpoint.x;

            if (hitpoint.x < paddleCenter.x)
            {
                ballrb.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
            else
            {
                ballrb.AddForce(new Vector2((Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
        }

    }
}
