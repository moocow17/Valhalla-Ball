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
    public bool gameIsOver;
    float startTime;
    private RespawnManager respawnManager;

    // Start is called before the first frame update
    void Start()
    {
        gamePlaying = false;
        gameIsOver = false;
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
        countdownDisplay.text = "GET READY!";

        yield return new WaitForSeconds(1f);

        while (countdownTime > 0)
        {
            AudioManager.instance.Play("KickStomp1", 1f, 1f, false);
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }
        
        AudioManager.instance.Play("VikingHorn", 1f, 1f, false);
        countdownDisplay.text = "GO!";

        startTime = Time.time;
        gamePlaying = true;

        yield return new WaitForSeconds(1f);

        countdownDisplay.gameObject.SetActive(false);

    }

    public void EndGame(string winner)
    {
        Debug.Log("EndGame");
        //STOP GAME PLAYING
        gamePlaying = false;
        gameIsOver = true;


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
        playAgainDisplay.text = "Play Again? (Press X + A)";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
