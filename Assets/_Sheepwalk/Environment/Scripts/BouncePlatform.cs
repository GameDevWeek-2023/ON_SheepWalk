using System;
using System.Collections;
using System.Collections.Generic;
using sheepwalk;
using UnityEngine;

public class BouncePlatform : MonoBehaviour
{
    private bool _active = false;
    private CharacterMovement _characterMovement;
    // time for squash down? Not much of an animation
    [SerializeField] private float animationDuration = 1f;
    private float _animationTime = 0f;
    private Vector3 _originalScale;
    private float _minYScaleFactor = 0.5f;
    private float _yScaleFactor = 1f;
    
    
    void Update()
    {
        if (!_active) return;
        if (_animationTime < 0)
        {
            _animationTime += Time.deltaTime;
            
        }
        else if (_animationTime < animationDuration)
        {
            _animationTime += 2 * Time.deltaTime;
            _characterMovement.velocity.y = 0;
        }
        else
        {
            _characterMovement.Jump(2*_characterMovement.jumpHeight);
            _active = false;
        }
        _yScaleFactor= _originalScale.y * Mathf.Lerp(_minYScaleFactor, 1f, Mathf.Abs(_animationTime) / animationDuration);
        transform.localScale = new Vector3(_originalScale.x, _originalScale.y*_yScaleFactor, _originalScale.z);

    }

    private void OnTriggerEnter(Collider other)
    {
        //check if player
        if (!_active)
        {

            _characterMovement = other.gameObject.GetComponent<CharacterMovement>();
            if (_characterMovement == null) return;

            var tags = _characterMovement.Pawn.GetComponent<CustomTags>();
            if (tags == null || !tags.HasTag("pawn") || !tags.HasTag("canBounce")) return;
            _active = true;
            _animationTime = -_animationTime;
            _originalScale = transform.localScale;
        }
    }
}
