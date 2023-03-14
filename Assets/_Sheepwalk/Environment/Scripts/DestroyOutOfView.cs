using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfView : MonoBehaviour
{
    private bool shouldDestroy = false;
    private void OnBecameInvisible()
    {
        shouldDestroy = true;
    }

    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(3);
        if (shouldDestroy)
        {
            Destroy(this);
        }
    }
}
