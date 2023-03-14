using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float depth = 0f;
    public int generatorState = 0;
    
    [SerializeField] private Camera _camera;
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private float closenessThreshold = 0.2f;
    
    private Dictionary<int, List<sheepwalk.StateTransitionDict.NodeTransition>> transitiongraph;
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
        var projectedPoint = _camera.WorldToViewportPoint(new Vector3(_xOffset, _camera.transform.position.y, depth));
        
    }

    bool checkProjectedPoint(Vector3 point)
    {
        return (point.z > 0 && point.x - 1 >= closenessThreshold);
    }
}
