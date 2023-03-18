using System;
using System.Collections;
using System.Collections.Generic;
using sheepwalk;
using UnityEngine;

public class CollectableSheep : MonoBehaviour
{
    [SerializeField] private string audioName;
    private void OnTriggerEnter(Collider other)
    {
        var tags = other.gameObject.GetComponent<CustomTags>();
        if (tags == null || !tags.HasTag("player")) return;
        var player = other.gameObject.GetComponent<CharacterSwitch>();
        if (player != null)
        {
            if (audioName != null) AudioManager.instance.Play(audioName);
            player.SwitchPawn(transform);
            //increase sheep score
            ScoreCounter.sheepCount += 1;
        }
    }
}
