using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 50 * Time.unscaledDeltaTime, 0);
    }
}
