using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace sheepwalk
{
    public class CharacterMovement : MonoBehaviour
    {
        public float gravity = -50f;
        public float runSpeed = 1f;
        public float jumpHeight = 2f;
        public Transform pawn;
        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private LayerMask obstacleLayers;
        [SerializeField] private List<Transform> groundChecks;
        [SerializeField] private List<Transform> wallChecks;
        [SerializeField] private PlayerDeath deathHandler;
        [SerializeField] private float speedIncreasePerSecond = 0.01f;
        [SerializeField] private LeaderPositionHistory _leaderPositionHistory;

        private float _hitCheckPrecision = 0.1f;
        private float _horizontalInput = 1f;
        private CharacterController _characterController;
        private Vector3 _velocity;
        private bool _isGrounded;
        private bool _hasHitObstacle;

        // Start is called before the first frame update
        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            
            var childrenWithTags = gameObject.GetComponentsInChildren<CustomTags>();

            foreach (var child in childrenWithTags)
            {
                if (child.HasTag("pawn"))
                {
                    pawn = child.transform;        
                    break;
                }
            }
             
        }

        // Update is called once per frame
        void Update()
        {
            runSpeed += speedIncreasePerSecond * Time.deltaTime;
            // Base Forward Movement
            _velocity.x = _horizontalInput * runSpeed;
            
            _isGrounded = false;
            foreach (var groundObject in groundChecks.Where(groundObject => Physics.CheckSphere(groundObject.position, _hitCheckPrecision, groundLayers, QueryTriggerInteraction.Ignore)))
            {
                _isGrounded = true;
                break;
            }
            
            if (_isGrounded && _velocity.y < 0)
            {
                //Debug.Log("Ground reached");
                _velocity.y = 0;
            }
            else
            {
                _velocity.y += gravity * Time.deltaTime;    
            }

            if (_isGrounded && Input.GetButtonDown("Jump"))
            {
                //Debug.Log("Jump");
                _velocity.y += -Mathf.Sign(gravity) * Mathf.Sqrt(Mathf.Abs(jumpHeight * 2 * gravity));
            }

            _characterController.Move(_velocity * Time.deltaTime);
            
            //Check for obstacle hit
            _hasHitObstacle = false;
            foreach (var obj in wallChecks.Where(groundObject => Physics.CheckSphere(groundObject.position, _hitCheckPrecision, obstacleLayers, QueryTriggerInteraction.Ignore)))
            {
                _hasHitObstacle = true;
                break;
            }
            
            // can check for tags of hit? OverlapSphere -> Collider -> Gameobject -> CustomTags
            // not needed?
            _leaderPositionHistory.Add(pawn.position);

            if (_hasHitObstacle)
            {
                Die();
                //Debug.Log("Hit obstacle. Should apply penalty");
            }
            
        }

        private void Die()
        {
            deathHandler.HandlePlayerDeath();
            
            //play some animation?
            
            Destroy(this);
        }
    }
}
