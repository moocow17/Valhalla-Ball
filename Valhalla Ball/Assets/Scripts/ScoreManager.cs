using MilkShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private GameController gameController;

    int whiteScore;
    int blackScore;
    public int whiteGoalsToWin = 5;
    public int blackGoalsToWin = 5;

    string winner;

    Text whiteScoreText;
    Text blackScoreText;

    public ShakePreset explosionShakePreset;
    public ShakePreset bigExplosionShakePreset;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GetComponent<GameController>();
        whiteScoreText = GameObject.Find("WhiteScore").GetComponent<Text>();
        blackScoreText = GameObject.Find("BlackScore").GetComponent<Text>();
        whiteScore = 0;
        blackScore = 0;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateScore(Goal goal)
    {
        //check which team scored
        if (goal.goalTeam == 0) //black scored
            blackScore++;
        if (goal.goalTeam == 1) //white scored
            whiteScore++;

        whiteScoreText.text = whiteScore.ToString();
        blackScoreText.text = blackScore.ToString();

        if(blackScore == blackGoalsToWin)
        {
            winner = "BLACK";
            GameWin(winner);
            
        }
        else if(whiteScore == whiteGoalsToWin)
        {
            Debug.Log("GameWin");
            winner = "WHITE";
            GameWin(winner);
        }
    }

    public void GameWin(string winner)
    {
        AudioManager.instance.Play("VikingHorn", 1f, 1f, false);
        Shaker.ShakeAll(explosionShakePreset);
        Shaker.ShakeAll(bigExplosionShakePreset);
        gameController.EndGame(winner);
    }
}
