using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCountdown : MonoBehaviour
{
    public int countdownTime;
    public Text countdownDisplay;
    public bool gamePlaying;
    float startTime; 

    // Start is called before the first frame update
    void Start()
    {
        gamePlaying = false;
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        while(countdownTime > 0)
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
