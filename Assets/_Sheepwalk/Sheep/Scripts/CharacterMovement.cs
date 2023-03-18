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
        
        // Activation Duration (short vs long jump)
        [SerializeField] private float minJumpActDuration = 0.1f;
        [SerializeField] private float maxJumpActDuration = 0.5f;
        //[SerializeField] private float minJumpHeightFactor = 0.3f;
        //[SerializeField] private float maxJumpHeightFactor = 1f;

        // Dash Parameters (only applicable if may dash)
        [SerializeField] private float dashDistance = 3f;
        [SerializeField] private float dashCD = 1f;
        [SerializeField] private float dashSpeedFactor = 3f;
        
        //Jump private params
        private bool _inJumpActivation = false;
        private float _jumpActivationDuration = 0f;
        // more like sqrt prev jump height
        private float _previousJumpHeight = 0f;
        
        //Dash private Params
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

        private bool _active = true;
        
        //Stuck detection
        private float _xProgress = 0f;
        private float _stuckDetectionThreshold = 2f;
        private float _stuckTimer = 0f;
        private float _eps = (float)1e-4;
        

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

        private Animator SheepAnimator
        {
            get
            {
                if (_pawn == null) return null;
                return _pawn.GetComponentInChildren<Animator>();
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
            if (!_active) return;
            runSpeed += speedIncreasePerSecond * Time.deltaTime;
            // Base Forward Movement
            velocity.x = _horizontalInput * runSpeed;
            
            _isGrounded = false;
            foreach (var groundObject in groundChecks.Where(groundObject => Physics.CheckSphere(groundObject.position, _hitCheckPrecision, groundLayers, QueryTriggerInteraction.Ignore)))
            {
                _isGrounded = true;
                break;
            }

            //stop jump animation
            if (_isGrounded && SheepAnimator != null) SheepAnimator.SetTrigger("stopFalling"); 

            if (_isGrounded && velocity.y < 0)
            {
                //Debug.Log("Ground reached");
                velocity.y = 0;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;    
            }

            if (_isGrounded && !_inJumpActivation && Input.GetButtonDown("Jump"))
            {
                _jumpActivationDuration = 0f;
                _previousJumpHeight = 0f;
                _inJumpActivation = true;
                AudioManager.instance.Play("Jump_2");
            }

            if (_inJumpActivation)
            {
                _jumpActivationDuration += Time.deltaTime;
                float jumpFactor;
                if (_jumpActivationDuration >= maxJumpActDuration || Input.GetButtonUp("Jump"))
                {
                    //Add rest
                    jumpFactor = Mathf.Lerp(0, 1f,
                        Mathf.Max(Time.deltaTime - Mathf.Max(0f, _jumpActivationDuration - maxJumpActDuration), minJumpActDuration - _jumpActivationDuration) / maxJumpActDuration);
                    _inJumpActivation = false;
                }
                else
                {
                    jumpFactor = Mathf.Lerp(0f, 1f, Time.deltaTime / maxJumpActDuration);
                    
                }
                _previousJumpHeight = IncJump(jumpHeight * jumpFactor, _previousJumpHeight);
            }

            if (_mayDash && _canDash && Input.GetButtonDown("Dash"))
            {
                if(SheepAnimator != null) SheepAnimator.SetTrigger("isDashing");
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
                AudioManager.instance.Play("Hit");
                Die();
                //Debug.Log("Hit obstacle. Should apply penalty");
            }

            if (transform.position.x <= _xProgress+_eps)
            {
                _stuckTimer += Time.deltaTime;
                if (_stuckTimer >= _stuckDetectionThreshold)
                {
                    Die();
                    return;
                }
            }
            else _stuckTimer = 0f;

            _xProgress = Mathf.Max(transform.position.x, _xProgress);
        }

        private void Die()
        {
            deathHandler.HandlePlayerDeath();

            //play some animation?
            if(SheepAnimator != null) {SheepAnimator.SetTrigger("knockedOut");}

            _active = false;
            
            StartCoroutine(nameof(DestroyOnDelay), 1f);

        }

        private IEnumerator DestroyOnDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(this);
        }

        public void Jump(float height)
        {
            // Todo: Scale Jumpheight inversely prop to movement speed? -> not really viable for height changes
            velocity.y += -FullSqrt(height * 2 * gravity);
            if(SheepAnimator != null) SheepAnimator.SetTrigger("isJumping");
        }
        
        public float IncJump(float additionalHeight, float previousFactor)
        {
            if (previousFactor == 0 && SheepAnimator != null)
            {
                SheepAnimator.SetTrigger("isJumping");
            }
            var c = FullSqrt(2 * gravity);
            var newFactor = FullSqrt(additionalHeight + Mathf.Sign(previousFactor)*Mathf.Pow(previousFactor, 2f));
            velocity.y += -c * (newFactor - previousFactor);
            return newFactor;
        }

        private float FullSqrt(float number)
        {
            return Mathf.Sign(number) * Mathf.Sqrt(Math.Abs(number));
        }
    }
}
