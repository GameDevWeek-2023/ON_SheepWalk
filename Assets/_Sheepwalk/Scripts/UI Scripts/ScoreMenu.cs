using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreMenu : MonoBehaviour
{
    public TMP_Text distanceScore;
    public TMP_Text snoozeNum;
    public TMP_Text sheepNum;
    public TMP_Text snoozeFinal;
    public TMP_Text sheepFinal;
    public TMP_Text finalScore;

    public int snoozeTotal;
    public int sheepTotal;

    public int snoozeMult;
    public int sheepMult;

    public int totalScore = 0;

    public GameObject scorePanel;

    public void DisplayScore()
    {
        print("score");
        scorePanel.gameObject.SetActive(true);
        //Display and Calculate Scores
        distanceScore.text = "" + ScoreCounter.distanceCount;
        snoozeNum.text = "" + ScoreCounter.snoozeCount;
        snoozeTotal = ScoreCounter.snoozeCount * snoozeMult;
        snoozeFinal.text = "" + snoozeTotal;

        totalScore = snoozeTotal + ScoreCounter.distanceCount; /*+sheepTotal*/
        finalScore.text = "" + totalScore;

        //Reset Scores
        ScoreCounter.distanceCount = 0;
        ScoreCounter.snoozeCount = 0;
    }
    public void RetryButton()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("GameLevel");
        print("RestartGame");
    }

    public void QuitToMenu()
     {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");
        print("Back to Menu");
     }
      
}
