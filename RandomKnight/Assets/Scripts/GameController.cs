using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text scoreText;
    public Text gameOverText;
    [HideInInspector]
    public static GameController instance;
    private bool gameOver = false;

    private int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver && Input.anyKeyDown)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public void PlayerScored()
    {
        score++;
        scoreText.text = "score: " + score.ToString();
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverText.enabled = true;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }
}
