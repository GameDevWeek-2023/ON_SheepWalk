using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreCounter : MonoBehaviour
{
    public static int snoozeCount;
    public TMP_Text snoozeText;
    public static int distanceCount = 0;
    public TMP_Text distText;

    public GameObject player;

    void Start()
    {
        distanceCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        snoozeText.text = "SC " + ScoreCounter.snoozeCount;

        distanceCount = Mathf.RoundToInt(player.transform.localPosition.x);
        distText.text = "Distance " + distanceCount;
    }
}
