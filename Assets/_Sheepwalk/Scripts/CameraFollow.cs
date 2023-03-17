using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target = null;
    private Vector3 _desiredTargetPos;

    private Vector3 _offset;
    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - target.position;
        _desiredTargetPos = _offset;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            _desiredTargetPos = _offset + new Vector3(target.position.x, 0, target.position.z);
            _desiredTargetPos.y = Mathf.Lerp(_desiredTargetPos.y,  target.position.y+_offset.y, Mathf.Abs(transform.position.y - target.position.y - _offset.y)/3);
                
        }
        transform.position = Vector3.Lerp(transform.position, _desiredTargetPos, Time.deltaTime*2);
        
    }
}
