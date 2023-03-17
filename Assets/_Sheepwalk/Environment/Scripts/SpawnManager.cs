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
        public float standardGap = 1.5f;
        public float scale = 1f;
        public int generatorState = 0;

        [SerializeField] private float initialOffset = 0f;
        [SerializeField] private Camera _referenceCamera;
        [SerializeField] private List<GameObject> prefabs;
        [SerializeField] private float closenessThreshold = 0.2f;

        private Dictionary<int, List<NodeTransition>> transitiongraph;
        private RandomizeSpawn _randomizeSpawn;
        private float _chosenScale;

        private float _xOffset;

        // Start is called before the first frame update
        void Start()
        {
            _randomizeSpawn = GetComponent<RandomizeSpawn>();   
            // Set to start state
            _xOffset = initialOffset;

            var transitionComponent = gameObject.GetComponent<StateTransitionDict>();
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
            if (shouldSpawn) SpawnPrefab();

        }

        public static bool CheckPointCloseRight(Vector3 point, float threshold)
        {
            return (point.z > 0f && point.x - 1 < threshold);
            //&& point.x >= 1f
        }
        
        public static bool CheckPointCloseLeft(Vector3 point, float threshold)
        {
            return (point.z > 0f && point.x > -threshold && point.x <= 0f);
        }
        
        public static bool CheckPointFarLeft(Vector3 point, float threshold)
        {
            return (point.z > 0f && point.x < -threshold);
        }
        
        void SpawnPrefab()
        {
            //Debug.Log("Should Spawn");
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
                Vector3 spawnPosition;
                if (_randomizeSpawn != null)
                {
                    spawnPosition = new Vector3(_xOffset, _randomizeSpawn.Height,
                        _randomizeSpawn.Depth);
                }
                else spawnPosition = new Vector3(_xOffset+standardGap, height, depth);

                
                //Debug.Log("Actually Attempting Spawn");
                var newSection = Instantiate(prefabs[generatorState], spawnPosition,
                    prefabs[generatorState].transform.rotation);
                // set new as child
                newSection.transform.parent = transform;
                // Scale
                _chosenScale = _randomizeSpawn != null ? _randomizeSpawn.Scale : scale;
                newSection.transform.localScale = Vector3.one * _chosenScale;

                //bounds scale??

                // adapt to work on simple collider objects? Or does add work?
                var bounds = newSection.GetComponent<PrefabAABB>().bounds;
                //Debug.Log(bounds);
                newSection.transform.Translate(-bounds.min.x*_chosenScale, 0, 0);
                
                _xOffset += bounds.size.x*_chosenScale;
                _xOffset += _randomizeSpawn != null ? _randomizeSpawn.Spacing : standardGap;
                // Is this centered or dependent on pivot?
                var destroyComponent = newSection.AddComponent<DestroyOutOfView>();
                //- bounds.center.x
                destroyComponent.Initialize(new Vector3(newSection.transform.position.x  + 2*bounds.extents.x*_chosenScale, spawnPosition.y, spawnPosition.z), _referenceCamera, closenessThreshold);

            }
        }
    }
}