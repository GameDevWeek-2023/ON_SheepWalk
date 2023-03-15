using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static sheepwalk.StateTransitionDict;
using Random = UnityEngine.Random;

namespace sheepwalk
{
    public class SpawnManager : MonoBehaviour
    {
        public float depth = 0f;
        public float height = 0f;
        public int generatorState = 0;

        [SerializeField] private float initialOffset = 0f;
        [SerializeField] private Camera _referenceCamera;
        [SerializeField] private List<GameObject> prefabs;
        [SerializeField] private float closenessThreshold = 0.2f;

        private Dictionary<int, List<NodeTransition>> transitiongraph;
        private float _xOffset;

        // Start is called before the first frame update
        void Start()
        {
            // Set to start state

            _xOffset = initialOffset;

            var transitionComponent = gameObject.GetComponent<sheepwalk.StateTransitionDict>();
            if (transitionComponent != null)
            {
                transitiongraph = transitionComponent.GetTransitionGraph();
            }
            // alternative?
            else
            {
                Debug.LogError("transition component not found");
            }
        }

        // Update is called once per frame
        void Update()
        {
            // One Object per Frame should suffice?
            var shouldSpawn =
                CheckPointCloseRight(_referenceCamera.WorldToViewportPoint(new Vector3(_xOffset, height, depth)),
                    closenessThreshold);
            if (shouldSpawn) spawnPrefab();

        }

        public static bool CheckPointCloseRight(Vector3 point, float threshold)
        {
            return (point.z > 0f && point.x - 1 < threshold && point.x >= 1f);
        }
        
        public static bool CheckPointCloseLeft(Vector3 point, float threshold)
        {
            return (point.z > 0f && point.x > -threshold && point.x <= 0f);
        }
        
        public static bool CheckPointFarLeft(Vector3 point, float threshold)
        {
            return (point.z > 0f && point.x < -threshold);
        }
        void spawnPrefab()
        {
            if (transitiongraph.ContainsKey(generatorState))
            {
                var transitions = transitiongraph[generatorState];
                var totalWeight = transitions.Sum(transition => transition.weight);

                var randWeight = Random.Range(0, totalWeight);
                var cumWeight = 0f;

                foreach (var transition in transitions)
                {
                    cumWeight += transition.weight;
                    if (randWeight < cumWeight)
                    {
                        generatorState = transition.adjacentNodeID;
                        break;
                    }
                }
                
            }
            
            // else?
            if (generatorState < prefabs.Count && generatorState >= 0)
            {
                var newSection = Instantiate(prefabs[generatorState], new Vector3(_xOffset, height, depth),
                    prefabs[generatorState].transform.rotation);
                
                // adapt to work on simple collider objects? Or does add work?
                var bounds = newSection.GetComponent<PrefabAABB>().bounds;
                newSection.transform.Translate(-bounds.min.x, 0, 0);
                _xOffset += bounds.size.x;
                // Is this centered or dependent on pivot?
                var destroyComponent = newSection.AddComponent<DestroyOutOfView>();
                destroyComponent.Initialize(new Vector3(newSection.transform.position.x - bounds.min.x, height, depth), _referenceCamera, closenessThreshold);
                    
            }
        }
    }
}