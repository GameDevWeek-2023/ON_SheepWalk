using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    public GameObject inGameMenu;

    public static bool gameOver = false;

    public IEnumerator DisplayScore()
    {
        yield return new WaitForSeconds(1);

        print("score");
        gameOver = true;
        scorePanel.gameObject.SetActive(true);
        inGameMenu.gameObject.SetActive(false);
        //Display and Calculate Scores
        distanceScore.text = "" + ScoreCounter.distanceCount;

        snoozeTotal = ScoreCounter.snoozeCount * snoozeMult;
        snoozeNum.text = "" + ScoreCounter.snoozeCount + " x " + snoozeMult;
        snoozeFinal.text = "= " + snoozeTotal;

        sheepTotal = ScoreCounter.sheepCount * sheepMult;
        sheepNum.text = "" + ScoreCounter.sheepCount + " x " + sheepMult;
        sheepFinal.text = "= " + sheepTotal;

        totalScore = snoozeTotal + ScoreCounter.distanceCount; /*+sheepTotal*/
        finalScore.text = "" + totalScore;

        //Reset Scores
        ScoreCounter.distanceCount = 0;
        ScoreCounter.snoozeCount = 0;
        ScoreCounter.sheepCount = 0;
    }
    public void RetryButton()
    {
        gameOver = false;
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        print("RestartGame");
    }

    public void QuitToMenu()
     {
        gameOver = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start Menu");
        print("Back to Menu");
     }
      
}
