using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sheepwalk
{

    public class LeaderPositionHistory : MonoBehaviour
    {
        public List<Vector3> PositionHistory;

        public int historyLength;

        public Transform target;

        // Start is called before the first frame update
        void Awake()
        {
            PositionHistory = new List<Vector3>();
        }

        // Update is called once per frame
        void Update()
        {
            if (target == null) return;
            PositionHistory.Add(target.position);
            if (PositionHistory.Count > historyLength)
            {
                PositionHistory.RemoveAt(0);
            }
        }
    }
}