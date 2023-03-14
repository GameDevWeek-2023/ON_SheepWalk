using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static sheepwalk.StateTransitionDict;

public class SpawnManager : MonoBehaviour
{
    public float depth = 0f;
    public float height = 0f;
    public int generatorState = 0;
    
    [SerializeField] private Camera _camera;
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private float closenessThreshold = 0.2f;
    
    private Dictionary<int, List<NodeTransition>> transitiongraph;
    private float _xOffset;

    // Start is called before the first frame update
    void Start()
    {
        // Set to start state

        _xOffset = 0f;
        
        var transitionComponent = gameObject.GetComponent<sheepwalk.StateTransitionDict>();
        if (transitionComponent != null)
        {
            transitiongraph = transitionComponent.GetTransitionGraph();
        } 
        // alternative?
    }

    // Update is called once per frame
    void Update()
    {
        var shouldSpawn = checkProjectedPoint(_camera.WorldToViewportPoint(new Vector3(_xOffset, height, depth)));
        if (shouldSpawn) spawnPrefab();

    }

    private bool checkProjectedPoint(Vector3 point)
    {
        return (point.z > 0 && point.x - 1 <= closenessThreshold);
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

            if (generatorState < prefabs.Count && generatorState >= 0)
            {
                var gameObject = Instantiate(prefabs[generatorState], new Vector3(_xOffset, height, depth), new Quaternion());
                //gameObject.AddComponent()
            }

        }
        // else?
    }
}
