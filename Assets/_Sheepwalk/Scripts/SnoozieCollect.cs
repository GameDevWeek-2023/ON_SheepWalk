using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnoozieCollect : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        //print("snooze time babey!");
        //if is Player 
        ScoreCounter.snoozeCount += 1;
        AudioManager.instance.Play("Snozzies_2");
        Destroy(this.gameObject);
        //deactivate/destroy game object
    }
}
