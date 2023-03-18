using System;
using System.Collections;
using System.Collections.Generic;
using sheepwalk.util;
using UnityEngine;

namespace sheepwalk
{

    public class LeaderPositionHistory : MonoBehaviour
    {
        public CyclicArray<Vector3> PositionHistory;
        public CyclicArray<float> Distances;
        [SerializeField] private int _historyLength;
        // should be readonly, but hard to set from editor
        public Transform target;

        private float _fpsEstimate = 20f;
        private const float FPSEstimateDecay = 0.99f;
        private const float Eps = (float)1e-6;
        public bool suggestSuicide = false;

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
            Distances = new CyclicArray<float>(HistoryLength);
        }

        public void Add(Vector3 position)
        {
            PositionHistory.Add(position);
            Distances.Add(0f);
            if (PositionHistory.Count > 1)
            {
                // x or magnitude?
                var currentStepLength = (PositionHistory[^1] - PositionHistory[^2]).magnitude;
                //var currentStepLength = Mathf.Abs((PositionHistory[^1] - PositionHistory[^2]).x);
                
                //Debug.Log(currentStepLength);
                for (int i = 0; i < Distances.Count-1; i++)
                {
                    Distances[i] += currentStepLength;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            _fpsEstimate = FPSEstimateDecay * _fpsEstimate + (1 - FPSEstimateDecay) / (Time.deltaTime + Eps);
            //if (target == null) return;
            //PositionHistory.Add(target.position);
            if (target != null) return;
            suggestSuicide = true;
        }
    }
}