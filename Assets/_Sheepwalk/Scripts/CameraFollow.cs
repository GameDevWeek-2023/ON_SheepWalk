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
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _desiredTargetPos = _offset + new Vector3(target.position.x, 0, target.position.z);
        if (Mathf.Abs(transform.position.y - target.position.y - _offset.y) > 3)
        {
            _desiredTargetPos += new Vector3(0, target.position.y, 0);
        }
        transform.position = Vector3.Lerp(transform.position, _desiredTargetPos, Time.deltaTime * 3);
    }
}
