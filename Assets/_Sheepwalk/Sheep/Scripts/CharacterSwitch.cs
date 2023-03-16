using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace sheepwalk
{
    public class CharacterSwitch : MonoBehaviour
    {
        [SerializeField] private Transform sheepHerd;

        [SerializeField] private float followDistance;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ReplaceWith(CustomTags child, CustomTags other)
        {
            child.Remove("pawn");
            child.Add("sheep");
            child.Add("follower");
            other.Remove("free");
            other.Add("pawn");
            PushBackHerd(followDistance);
            child.transform.parent = sheepHerd.transform;
            var follow = child.AddComponent<FollowLeader>();
            follow.optimalSecondDistance = followDistance;
            //follow.offset = child.transform.position - transform.position;
            // move to other
            transform.position = other.transform.position;

            other.transform.parent = transform;
            
        }

        private void PushBackHerd(float amount)
        {
            var children = sheepHerd.GetComponentsInChildren<FollowLeader>();
            foreach (var child in children)
            {
                child.optimalSecondDistance += amount;
            }
        }

        private void FillFollowSkip(Vector3 skip)
        {
            var positionHistory = sheepHerd.GetComponent<LeaderPositionHistory>();
            //Todo: Add positions -> requires some speed in distance...
        }

        public void SwitchPawn(Transform other)
        {
            var tags = other.gameObject.GetComponent<CustomTags>();
            if (tags == null || !tags.HasTag("free")) return;
            
            var childrenWithTags = gameObject.GetComponentsInChildren<CustomTags>();

            foreach (var child in childrenWithTags)
            {
                if (child.HasTag("pawn"))
                {
                    ReplaceWith(child, tags);        
                    break;
                }
            }
        }
    }
}