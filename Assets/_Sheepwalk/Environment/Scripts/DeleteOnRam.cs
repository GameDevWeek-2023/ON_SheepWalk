using System;
using System.Collections;
using System.Collections.Generic;
using sheepwalk;
using UnityEngine;

public class DeleteOnRam : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var characterMovement = other.gameObject.GetComponent<CharacterMovement>();
        Debug.Log("DeleteOnRam Triggered");
        if (characterMovement == null) return;

        if (characterMovement.IsDashing)
        {
            Debug.Log("Should Destroy");
            Destroy(gameObject);
        }
    }
}
