using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  sheepwalk
{
    public class StateTransitionDict : MonoBehaviour
    {
        [System.Serializable]
        public struct NodeTransition
        {
            public int adjacentNodeID;
            public float weight;
        }
        
        [System.Serializable]
        public struct AdjacentNode
        {
            public int currentNodeID;
            public NodeTransition transition;
        }

        [SerializeField] private List<AdjacentNode> stateTransitionList;

        private Dictionary<int, List<NodeTransition>> transitiongraph;
        // Start is called before the first frame update
        void Start()
        {
            InitializeTransitionGraph();
        }

        private void InitializeTransitionGraph()
        {
            if (transitiongraph != null) return;
            transitiongraph = new Dictionary<int, List<NodeTransition>>();
            
            foreach (var node in stateTransitionList)
            {
                if (transitiongraph.ContainsKey(node.currentNodeID))
                {
                    transitiongraph[node.currentNodeID].Add(node.transition);
                }
                else
                {
                    var l = new List<NodeTransition>();
                    l.Add(node.transition);
                    transitiongraph.Add(node.currentNodeID, l);
                }
            }
        }

        public Dictionary<int, List<NodeTransition>> GetTransitionGraph()
        {
            InitializeTransitionGraph();
            return transitiongraph;
        }
    }    
}

