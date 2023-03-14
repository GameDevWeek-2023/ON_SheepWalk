using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sheepwalk
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayers;
        public float gravity = -50f;
        public float runSpeed = 1f;
        public float jumpHeight = 2f;

        private float _horizontalInput = 1f;
        private CharacterController _characterController;
        private Vector3 _velocity;
        private bool _isGrounded;
        
        // Start is called before the first frame update
        void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            _velocity.x = _horizontalInput * runSpeed;
            _isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);
            
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
                _velocity.y += Mathf.Sqrt(Mathf.Abs(jumpHeight * 2 * gravity));
            }

            // _characterController.Move(new Vector3(_horizontalInput * runSpeed * Time.deltaTime, 0, 0));
            _characterController.Move(_velocity * Time.deltaTime);
            
        }
    }
}
