using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int countdownTime;
    public Text countdownDisplay;
    public Text playAgainDisplay;
    public bool gamePlaying;
    float startTime;
    private RespawnManager respawnManager;

    // Start is called before the first frame update
    void Start()
    {
        gamePlaying = false;
        playAgainDisplay.text = "";
        respawnManager = GetComponent<RespawnManager>();
        StartCoroutine(CountdownToStart());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        countdownDisplay.text = "GO!";

        startTime = Time.time;
        gamePlaying = true;

        yield return new WaitForSeconds(1f);

        countdownDisplay.gameObject.SetActive(false);

    }

    public void EndGame(string winner)
    {
        //STOP GAME PLAYING
        gamePlaying = false;

        //DESTROY PLAYERS
        respawnManager.DestroyAllPlayers();

        //CHANGE TEXT TO SHOW WHO WON (and play audio)
        if (winner == "WHITE")
            countdownDisplay.color = new Color(255, 255, 255);
        else if (winner == "BLACK")
            countdownDisplay.color = new Color(0, 0, 0);
        countdownDisplay.text = winner + " WINS!";
        countdownDisplay.gameObject.SetActive(true);

        //DISPLAY TEXT TO RESTART GAME
        playAgainDisplay.text = "Play Again? (Press X or A)";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
