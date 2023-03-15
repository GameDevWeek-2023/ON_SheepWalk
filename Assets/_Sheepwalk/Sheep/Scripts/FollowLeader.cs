using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLeader : MonoBehaviour
{
    public float optimalSecondDistance = 0.2f;
    [SerializeField] private sheepwalk.LeaderPositionHistory _leaderHistory;

    private float fpsEstimate = 20f;
    private float fpsEstimateDecay = 0.9f;
    private float eps = (float)1e-6;
    private int currentIndex = 0;
    private int targetIndex = 0;
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - _leaderHistory.target.transform.position;
        offset.x = 0;
    }

    // Update is called once per frame
    void Update()
    {
        fpsEstimate = fpsEstimateDecay * fpsEstimate + (1 - fpsEstimateDecay) / (Time.deltaTime + eps);
        targetIndex = _leaderHistory.historyLength - 1 - Mathf.CeilToInt(fpsEstimate * optimalSecondDistance);

        if (_leaderHistory.PositionHistory.Count > 0)
        {
            currentIndex -= 1;
            if (currentIndex < targetIndex)
            {
                currentIndex++;
                if (currentIndex < targetIndex) currentIndex++;
            }

            currentIndex = Math.Min(Math.Max(0, currentIndex), _leaderHistory.PositionHistory.Count-1);

            transform.position = _leaderHistory.PositionHistory[currentIndex] + offset;
        }

    }
}
