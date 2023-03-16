using System;
using System.Collections;
using System.Collections.Generic;
using sheepwalk;
using UnityEngine;
using UnityEngine.Serialization;

public class FollowLeader : MonoBehaviour
{
    public float optimalSecondDistance = 0.2f;
    [SerializeField] private sheepwalk.LeaderPositionHistory _leaderHistory;
    
    public int currentIndex = 0;
    private int _targetIndex = 0;
    public Vector3 offset;

    private void Start()
    {
        var leaderHistory = transform.parent.gameObject.GetComponent<LeaderPositionHistory>();
        if (leaderHistory != null)
        {
            _leaderHistory = leaderHistory;
        }
        offset = transform.position - _leaderHistory.target.transform.position;
        offset.x = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _targetIndex = _leaderHistory.HistoryLength - 1 - Mathf.CeilToInt(_leaderHistory.FPSEstimate * optimalSecondDistance);

        if (_leaderHistory.PositionHistory.Count > 0)
        {
            currentIndex -= 1;
            if (currentIndex < _targetIndex)
            {
                currentIndex++;
                if (currentIndex < _targetIndex) currentIndex++;
            }

            currentIndex = Math.Min(Math.Max(0, currentIndex), _leaderHistory.PositionHistory.Count-1);

            transform.position = _leaderHistory.PositionHistory[currentIndex] + offset;
        }
    }
}
