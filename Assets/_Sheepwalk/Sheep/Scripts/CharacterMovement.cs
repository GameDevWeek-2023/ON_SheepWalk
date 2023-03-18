using System;
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
        private Transform _pawn;
        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private LayerMask obstacleLayers;
        [SerializeField] private List<Transform> groundChecks;
        [SerializeField] private List<Transform> wallChecks;
        [SerializeField] private PlayerDeath deathHandler;
        [SerializeField] private float speedIncreasePerSecond = 0.05f;
        [SerializeField] private LeaderPositionHistory _leaderPositionHistory;
        [SerializeField] private float dashDistance = 3f;
        [SerializeField] private float dashCD = 1f;
        [SerializeField] private float dashSpeedFactor = 3f;
        private bool _mayDash = false;
        private bool _canDash = false;
        private float _remainingDashDistance = 0f;
        private float _remainingDashCD = 0f;
        private bool _wasGrounded = false;
        
        private float _hitCheckPrecision = 0.1f;
        private float _horizontalInput = 1f;
        private CharacterController _characterController; 
        public Vector3 velocity;
        private bool _isGrounded;
        private bool _hasHitObstacle;

        public Animator normieAnim;

        public static bool sheepSwitch;

        public Transform Pawn
        {
            get => _pawn;
            set
            {
                _pawn = value;
                var tags = value.GetComponent<CustomTags>();
                if (tags != null) _mayDash = tags.HasTag("canDash");
            }
        }

        public bool IsDashing => _remainingDashDistance > 0;

        // Start is called before the first frame update
        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            
            var childrenWithTags = gameObject.GetComponentsInChildren<CustomTags>();

            foreach (var child in childrenWithTags)
            {
                if (child.HasTag("pawn"))
                {
                    Pawn = child.transform;
                    break;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            runSpeed += speedIncreasePerSecond * Time.deltaTime;
            // Base Forward Movement
            velocity.x = _horizontalInput * runSpeed;
            
            _isGrounded = false;
            foreach (var groundObject in groundChecks.Where(groundObject => Physics.CheckSphere(groundObject.position, _hitCheckPrecision, groundLayers, QueryTriggerInteraction.Ignore)))
            {
                _isGrounded = true;
                break;
            }
            
            if (_isGrounded && velocity.y < 0)
            {
                //Debug.Log("Ground reached");
                velocity.y = 0;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;    
            }

            if (_isGrounded && Input.GetButtonDown("Jump"))
            {
                //Debug.Log("Jump");
                Jump(jumpHeight);
            }

            if (_mayDash && _canDash && Input.GetButtonDown("Dash"))
            {
                _remainingDashDistance = dashDistance;
                _canDash = false;
                _remainingDashCD = dashCD;
            }

            if (_remainingDashDistance > 0)
            {
                velocity.x += _horizontalInput * runSpeed * dashSpeedFactor;
            }

            _characterController.Move(velocity * Time.deltaTime);
            
            if (!_canDash)
            {
                _remainingDashCD -= Time.deltaTime;
                _remainingDashDistance -= velocity.x * Time.deltaTime;
                if (_isGrounded) _wasGrounded = true;
                if (_wasGrounded && _remainingDashDistance <= 0f && _remainingDashCD <= 0f)
                {
                    _canDash = true;
                }
            }
            
            //Check for obstacle hit
            _hasHitObstacle = false;
            foreach (var obj in wallChecks.Where(groundObject => Physics.CheckSphere(groundObject.position, _hitCheckPrecision, obstacleLayers, QueryTriggerInteraction.Ignore)))
            {
                _hasHitObstacle = true;
                break;
            }
            
            // can check for tags of hit? OverlapSphere -> Collider -> Gameobject -> CustomTags
            // not needed?
            _leaderPositionHistory.Add(_pawn.position);

            if (!IsDashing && _hasHitObstacle)
            {
                Die();
                //Debug.Log("Hit obstacle. Should apply penalty");
            }

            
        }

        private void Die()
        {
            deathHandler.HandlePlayerDeath();

            //play some animation?
            normieAnim.SetTrigger("knockedOut");
            Destroy(this);
        }

        public void Jump(float height)
        {
            normieAnim.SetTrigger("isJumping");
            velocity.y += -Mathf.Sign(gravity) * Mathf.Sqrt(Mathf.Abs(height * 2 * gravity));
        } 
    }
}
