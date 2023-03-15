using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneFollow : MonoBehaviour
{

    public float height = -5f;
    [SerializeField] private float _followDelay = 2f;
    [SerializeField] private Camera _camera;
    private bool _running = true;
    
    void Start()
    {
        StartCoroutine(nameof(FollowAfterDelay));
    }

    //Slow Follow
    private IEnumerator FollowAfterDelay()
    {
        while (_running)
        {
            yield return new WaitForSeconds(_followDelay);
            transform.position = new Vector3(_camera.transform.position.x, height, 0f);
            //Debug.Log("Death Zone Position Update");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var tags = other.gameObject.GetComponent<sheepwalk.CustomTags>();
        if (tags == null || tags.HasTag("sheep") && tags.HasTag("follower"))
        {
            Destroy(other.gameObject);
        }
        else if (tags.HasTag("player"))
        {
            Destroy(other.gameObject);
            // kill player -> new component
            var deathComponent = gameObject.GetComponent<sheepwalk.PlayerDeath>();
            deathComponent.HandlePlayerDeath();
        }
        
    }
}

    