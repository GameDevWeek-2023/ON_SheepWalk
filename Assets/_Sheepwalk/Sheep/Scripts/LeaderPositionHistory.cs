using System.Collections;
using System.Collections.Generic;
using sheepwalk.util;
using UnityEngine;

namespace sheepwalk
{

    public class LeaderPositionHistory : MonoBehaviour
    {
        public CyclicArray<Vector3> PositionHistory;
        [SerializeField] private int _historyLength;
        // should be readonly, but hard to set from editor
        public Transform target;
        
        private float _fpsEstimate = 20f;
        private const float FPSEstimateDecay = 0.9f;
        private const float Eps = (float)1e-6;

        public int HistoryLength
        {
            get { return _historyLength; }
        }

        public float FPSEstimate
        {
            get { return _fpsEstimate; }
        }

        // Start is called before the first frame update
        void Awake()
        {
            PositionHistory = new CyclicArray<Vector3>(HistoryLength);
        }

        // Update is called once per frame
        void Update()
        {
            _fpsEstimate = FPSEstimateDecay * _fpsEstimate + (1 - FPSEstimateDecay) / (Time.deltaTime + Eps);
            if (target == null) return;
            PositionHistory.Add(target.position);
        }
    }
}