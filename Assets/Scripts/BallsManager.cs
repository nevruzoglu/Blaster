using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsManager : MonoBehaviour
{

    #region  Singleton


    private static BallsManager _instance;
    public static BallsManager Instance => _instance;


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

    [SerializeField]
    private Ball ballPrefab;

    private Ball initialBall;
    private Rigidbody2D initialBallrb;
    public float initialBallSpeed = 450;

    public List<Ball> Balls { get; set; }



    private void Start()
    {
        InitBall();
    }

    private void Update()
    {

        if (!GameManager.Instance.IsGameStarted) 
        {
            Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0);
            initialBall.transform.position = ballPosition;

            if (Input.GetMouseButtonDown(0)) // mouse basılınca topun hareket etmesi
            {
                initialBallrb.isKinematic = false;
                initialBallrb.AddForce(new Vector2(0, initialBallSpeed));
                GameManager.Instance.IsGameStarted = true;
            }
        }
    }

    private void InitBall() // topun platforma başlangıçta yapışması
    {
        Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
        Vector3 startingPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0); // getting from paddle
        initialBall = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
        initialBallrb = initialBall.GetComponent<Rigidbody2D>();


        this.Balls = new List<Ball>{
        initialBall
};
    }
}
