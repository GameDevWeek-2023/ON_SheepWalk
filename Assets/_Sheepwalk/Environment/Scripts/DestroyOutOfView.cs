using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfView : MonoBehaviour
{
    private Vector3 _rightEdgePoint;
    private Camera _camera;
    private float _threshold;

    public void Initialize(Vector3 point, Camera cam, float threshold)
    {
        _rightEdgePoint = point;
        _camera = cam;
        _threshold = threshold;
    }
    
    // may still be too costly?
    void Update()
    {
        if (sheepwalk.SpawnManager.CheckPointFarLeft(_camera.WorldToViewportPoint(_rightEdgePoint), _threshold))
        {
            Destroy(gameObject);
        }
    }
}
