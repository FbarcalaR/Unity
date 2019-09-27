using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [HideInInspector]
    public static GameController instance;
    public  Text scoreTeamAText;
    public Text scoreTeamBText;
    public Text teamScored;

    private int scoreTeamA;
    private int scoreTeamB;
    private bool gameOver;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            scoreTeamA = 0;
            scoreTeamB = 0;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(instance);

        instance.gameOver = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
            scoreTeamAText.enabled = false;
            scoreTeamBText.enabled = false;
        }
    }

    public void TeamScored(GameObject whereScored)
    {
        
        if (whereScored.tag == "ScoreZoneTeamA")
        {
            Debug.Log("Team A scored");
            teamScored.text = "Team A Scored!";
            scoreTeamA++;
        }
        else if (whereScored.tag == "ScoreZoneTeamB")
        {
            Debug.Log("Team B scored");
            teamScored.text = "Team B Scored!";
            scoreTeamB++;
        }

        scoreTeamAText.text = "Team A: " + scoreTeamA;
        scoreTeamBText.text = "Team B: " + scoreTeamB;
        teamScored.enabled = true;

        Invoke("ReloadLevel", 2f);
    }

    private void ReloadLevel()
    {
        teamScored.enabled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        gameOver = true;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

}
