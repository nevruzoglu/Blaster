using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region  Singleton 


    private static GameManager _instance;
    public static GameManager Instance => _instance;


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

    public GameObject gameOverScreen;
    public int AvailableLives = 3;
    public int Lives { get; set; }

    public bool IsGameStarted { get; set; }

    private void Start()
    {
        this.Lives = this.AvailableLives;
        Screen.SetResolution(640, 960, false);
        Ball.OnBallDeath += OnBallDeath;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnBallDeath(Ball obj)
    {
        if (BallsManager.Instance.Balls.Count <= 0)
        {

            this.Lives--;

            if (this.Lives < 1)
            {
                gameOverScreen.SetActive(true);
            }
            else
            {

                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
                BricksManager.Instance.LoadLevel(BricksManager.Instance.CurrentLevel);

            }
        }
    }
    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;

    }
}
