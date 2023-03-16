using System;
using sheepwalk;
using UnityEngine;

public class FollowLeader : MonoBehaviour
{
    public float optimalDistance = 1f;
    [SerializeField] private LeaderPositionHistory _leaderHistory;
    
    public int currentIndex = 0;
    //private int _targetIndex = 0;
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
        //_targetIndex = _leaderHistory.HistoryLength - 1 - Mathf.CeilToInt(_leaderHistory.FPSEstimate * optimalDistance/5);

        if (_leaderHistory.PositionHistory.Count > 0)
        {
            currentIndex -= 1;
            //if (currentIndex < _targetIndex)
            if (ShouldGetCloser(currentIndex))
            {
                currentIndex++;
                if (ShouldGetCloser(currentIndex)) currentIndex++;
                //if (currentIndex < _targetIndex) currentIndex++;
            }

            currentIndex = Math.Min(Math.Max(1, currentIndex), _leaderHistory.PositionHistory.Count-1);

            if (_leaderHistory.Distances[currentIndex] < optimalDistance)
            {
                transform.position = Vector3.Lerp(_leaderHistory.PositionHistory[currentIndex-1], _leaderHistory.PositionHistory[currentIndex], 
                    (optimalDistance-_leaderHistory.Distances[currentIndex-1])/(_leaderHistory.Distances[currentIndex]-_leaderHistory.Distances[currentIndex-1] + 0.0001f));
            }
            else
            {
                transform.position = _leaderHistory.PositionHistory[currentIndex] + offset;   
            }
        }
    }

    private bool ShouldGetCloser(int index)
    {
        if (index < 1) return true;
        if (index >= _leaderHistory.HistoryLength - 1) return false;

        return _leaderHistory.Distances[index] > optimalDistance;
            //&& (_leaderHistory.Distances[index + 1] + _leaderHistory.Distances[index]) / 2 > optimalDistance;

    } 
}
