using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    int whiteScore;
    int blackScore;

    Text whiteScoreText;
    Text[] scoreTexts;
    Text blackScoreText;


    // Start is called before the first frame update
    void Start()
    {
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
    }
}
