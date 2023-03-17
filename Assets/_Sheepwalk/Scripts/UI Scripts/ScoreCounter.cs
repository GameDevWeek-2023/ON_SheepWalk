using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreCounter : MonoBehaviour
{
    public static int snoozeCount;
    public TMP_Text snoozeText;

    public static int sheepCount = 0;
    public TMP_Text sheepText;

    public static int distanceCount = 0;
    public int distMult;
    public TMP_Text distText;

    public GameObject player;

    void Start()
    {
        distanceCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        snoozeText.text = "Zees " + snoozeCount;

        sheepText.text = "Sheep " + sheepCount;
        if (player != null) distanceCount = Mathf.RoundToInt(player.transform.localPosition.x)*distMult;
        distText.text = "" + distanceCount;
    }
}
