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

        private void ReplaceWith(CustomTags child, CustomTags other)
        {
            child.Remove("pawn");
            child.Add("sheep");
            child.Add("follower");
            other.Remove("free");
            other.Add("pawn");
            PushBackHerd(followDistance);
            
            Vector3 offsetVector = child.transform.localPosition;
            child.transform.parent = sheepHerd.transform;
            var follow = child.AddComponent<FollowLeader>();
            LeaderPositionHistory historyComponent = sheepHerd.GetComponent<LeaderPositionHistory>();
            follow.currentIndex = historyComponent.PositionHistory.Count-2;
            follow.optimalDistance = followDistance;
            child.transform.localScale.Scale(Vector3.one*0.7f);
            var comp = child.gameObject.GetComponent<Collider>(); 
            if (comp != null) comp.enabled = false;
            //var comp2 = child.gameObject.GetComponent<CollectableSheep>(); 
            //if (comp2 != null) comp2.enabled = false;
            //follow.offset = child.transform.position - transform.position;
            follow.offset = offsetVector;
            //child.transform.position = offsetVector;
            // move to other
            transform.position = other.transform.position;
            //Debug.Log(historyComponent.Distances[^1]);
            //Debug.Log(historyComponent.Distances[^2]);
            //Debug.Log(historyComponent.Distances[^3]);
            // push up to foot?

            other.transform.parent = transform;
            
        }

        private void PushBackHerd(float amount)
        {
            var children = sheepHerd.GetComponentsInChildren<FollowLeader>();
            foreach (var child in children)
            {
                child.optimalDistance += amount;
            }
        }

        private void FillFollowSkip(Vector3 skip)
        {
            var positionHistory = sheepHerd.GetComponent<LeaderPositionHistory>();
            var charMovement = gameObject.GetComponent<CharacterMovement>();
            // Unsure if can be equal
            var lastPos = positionHistory.PositionHistory[^1];
            //should be child pos. what am i even logging??
            var distance = transform.position - lastPos;
            var numberFrames = distance.magnitude / charMovement.runSpeed * positionHistory.FPSEstimate;


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