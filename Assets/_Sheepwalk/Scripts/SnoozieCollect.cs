using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnoozieCollect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("snooze time babey!");
        //if is Player 
        ScoreCounter.snoozeCount += 1;
        Destroy(this.gameObject);
        //deactivate/destroy game object
    }
}
