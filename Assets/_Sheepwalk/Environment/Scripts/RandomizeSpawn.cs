using System.Collections;
using System.Collections.Generic;
using sheepwalk;
using UnityEngine;

namespace sheepwalk
{
    public class RandomizeSpawn : MonoBehaviour
    {
        private SpawnManager _spawnManager;
        
        [SerializeField] private bool randomizeDepth;
        [SerializeField] private float depthMin;
        [SerializeField] private float depthMax;
        
        [SerializeField] private bool randomizeHeight;
        [SerializeField] private float heightMin;
        [SerializeField] private float heightMax;
        
        [SerializeField] private bool randomizeSpacing;
        [SerializeField] private float spacingMin;
        [SerializeField] private float spacingMax;
        
        [SerializeField] private bool randomizeScale;
        [SerializeField] private float scaleMin;
        [SerializeField] private float scaleMax;
        public float Height
        {
            get
            {
                if (randomizeHeight)
                {
                    return _spawnManager.height+Random.Range(heightMin, heightMax);
                }
                return _spawnManager.height;
            }
        }
        
        public float Depth
        {
            get
            {
                if (randomizeDepth)
                {
                    return _spawnManager.depth+Random.Range(depthMin, depthMax);
                }
                return _spawnManager.depth;
            }
        }
        
        public float Spacing
        {
            get
            {
                if (randomizeSpacing)
                {
                    return _spawnManager.standardGap+Random.Range(spacingMin, spacingMax);
                }
                return _spawnManager.standardGap;
            }
        }
        
        public float Scale
        {
            get
            {
                if (randomizeScale)
                {
                    return _spawnManager.scale*Random.Range(scaleMin, scaleMax);
                }
                return _spawnManager.scale;
            }
        }
        

        // Start is called before the first frame update
        void Start()
        {
            _spawnManager = GetComponent<SpawnManager>();
            
        }
        
    }
}
