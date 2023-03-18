using System;
using sheepwalk;
using UnityEngine;

public class FollowLeader : MonoBehaviour
{
    public float optimalDistance = 1f;
    [SerializeField] private LeaderPositionHistory leaderHistory;
    
    public int currentIndex = 0;
    //private int _targetIndex = 0;
    public Vector3 offset;

    private void Start()
    {
        var leaderHistory = transform.parent.gameObject.GetComponent<LeaderPositionHistory>();
        if (leaderHistory != null)
        {
            this.leaderHistory = leaderHistory;
            offset = transform.position - leaderHistory.target.transform.position;
            // target can be unassigned
            offset.x = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //_targetIndex = _leaderHistory.HistoryLength - 1 - Mathf.CeilToInt(_leaderHistory.FPSEstimate * optimalDistance/5);

        if (leaderHistory.PositionHistory.Count <= 0) return;
        if (leaderHistory.suggestSuicide)
        {
            if (currentIndex < leaderHistory.PositionHistory.Count - 1)
            {
                currentIndex++;
                currentIndex = Math.Min(Math.Max(1, currentIndex), leaderHistory.PositionHistory.Count-1);
                transform.position = leaderHistory.PositionHistory[currentIndex] + offset;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            currentIndex -= 1;
            //if (currentIndex < _targetIndex)
            if (ShouldGetCloser(currentIndex))
            {
                currentIndex++;
                if (ShouldGetCloser(currentIndex)) currentIndex++;
                //if (currentIndex < _targetIndex) currentIndex++;
            }


            currentIndex = Math.Min(Math.Max(1, currentIndex), leaderHistory.PositionHistory.Count - 1);

            if (leaderHistory.Distances[currentIndex] < optimalDistance)
            {
                transform.position = Vector3.Lerp(leaderHistory.PositionHistory[currentIndex - 1],
                    leaderHistory.PositionHistory[currentIndex],
                    (optimalDistance - leaderHistory.Distances[currentIndex - 1]) /
                    (leaderHistory.Distances[currentIndex] - leaderHistory.Distances[currentIndex - 1] + 0.0001f));
            }
            else
            {
                transform.position = leaderHistory.PositionHistory[currentIndex] + offset;
            }
        }
    }

    private bool ShouldGetCloser(int index)
    {
        if (index < 1) return true;
        if (index >= leaderHistory.HistoryLength - 1) return false;

        return leaderHistory.Distances[index] > optimalDistance;
            //&& (_leaderHistory.Distances[index + 1] + _leaderHistory.Distances[index]) / 2 > optimalDistance;

    } 
}
