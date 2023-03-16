using UnityEngine;
using System.Collections;
 
// https://answers.unity.com/questions/333001/getting-bounds-of-a-prefab-with-multiple-children.html
[ExecuteInEditMode]
public class PrefabAABB : MonoBehaviour
{
    /// <summary>
    /// local space Axis Aligned Bounding Box
    /// </summary>
    public Bounds bounds;
    private Transform _transform;
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + bounds.center, bounds.size);
    }
 
    void Reset ()
    {
        RecalculateBounds();
    }
 #if False
    public Bounds WorldBounds ()
    {
        if (_transform == null)
            _transform = transform;
 
        Bounds b = bounds;
        b.center += _transform.position;
 
        Vector3 size = b.size;
        Vector3 tsize = _transform.lossyScale;
        size.x *= tsize.x;
        size.y *= tsize.y;
        size.z *= tsize.z;
        b.size = size;
 
        return b;
    }
#endif
 
    //[ContextMenu("Recalculate Bounds")]
    public void RecalculateBounds ()
    {
        var thisMeshRenderer = GetComponent<MeshRenderer>();
        if (thisMeshRenderer == null)
        {
            bounds = new Bounds(transform.position, Vector3.zero);
        }
        else
        {
            bounds = thisMeshRenderer.bounds;
        }
        var meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var mr in meshRenderers)
        {
            //var pos = mf.transform.localPosition;
            Bounds childBounds = mr.bounds;
            //childBounds.center += pos;
            /*if (gameObject.name == "Small Gap - Glossy Green")
            {
                Debug.Log(mr.bounds.center);
                Debug.Log(mr.bounds.size);
            }*/
            bounds.Encapsulate(childBounds);
        }

        bounds.center -= transform.position;
    }
 
#if (UNITY_EDITOR)
     void Update()
     {
         if (Application.isPlaying) return;
         RecalculateBounds();
     }
#endif
}