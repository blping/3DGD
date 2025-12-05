    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        // NEW: Public flag to switch logic (you'd control this with your V key script)
        public bool isThirdPerson = true; 
        
        // NEW: Reference to the external camera pivot object (the Neck)
        public Transform cameraTarget;
        public float walkSpeed = 6f;
        public float runSpeed = 12f;
        public float jumpPower = 7f;
        public float gravity = 10f;

        public float defaultHeight = 2f;
        public float crouchHeight = 1f;
        public float crouchSpeed = 3f;

        private Vector3 moveDirection = Vector3.zero;

        private CharacterController characterController;

        private bool canMove = true;

        private Animator animator;

        void Start()
        {
            Debug.Log("Hello, this is a console message!");
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButtonDown("Jump") && canMove && characterController.isGrounded)
            {
                Debug.Log("wtf");
                animator.SetTrigger("Jumping");
                moveDirection.y = jumpPower;
            }
            else
            {
                moveDirection.y = movementDirectionY;
                
            }

            if (!characterController.isGrounded)
            {
                
                
                moveDirection.y -= gravity * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.R) && canMove)
            {   
                characterController.height = crouchHeight;
                walkSpeed = crouchSpeed;
                runSpeed = crouchSpeed;

            }
            else
            {
                characterController.height = defaultHeight;
                walkSpeed = 6f;
                runSpeed = 12f;
            }

            characterController.Move(moveDirection * Time.deltaTime);

            if (canMove)
            {
                if (isThirdPerson)
                {
                    // ... (Your existing Third-Person movement-based rotation code) ...

                    Debug.Log("Hello, Player Movement isThirdPerson works!");

                    /*

                    if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                    {
                        Debug.Log("Hello, Player Movement isThirdPerson a works!");
                        Quaternion targetRotation = Quaternion.Euler(0, cameraTarget.eulerAngles.y, 0);
                        transform.rotation = Quaternion.Slerp(
                            transform.rotation, 
                            targetRotation, 
                            Time.deltaTime * 50f
                        );
                    }
                    */
                    
                }
                else // isThirdPerson == false (First-Person View)
                {
                    // **FIRST-PERSON LOOK:** Player body MUST rotate with the mouse!
                    /*
                    Debug.Log("Hello, Player Movement isFirstPerson a works!");

                    // Get horizontal mouse input and rotate the player body directly.
                    float mouseX = Input.GetAxis("Mouse X") * 400f * Time.deltaTime; // Use a high speed
                    transform.rotation *= Quaternion.Euler(0, mouseX, 0);
                    */
                    // Vertical camera rotation is handled by the CameraFollowTarget script on the Neck.
                }
                // If NOT isThirdPerson (i.e., First-Person view)
                // We assume the CameraFollowTarget.cs script on the Neck is handling the rotation
                // of the entire Player object when in First-Person mode, OR the Cinemachine VCam
                // is simply rotating the neck/head.
                // Therefore, no rotation logic is needed here.

                // --- ANIMATION CONTROL ---
                if (animator != null)
                {
                    // Calculate the magnitude of horizontal movement
                    float horizontalVelocity = new Vector3(
                        characterController.velocity.x, 
                        0, 
                        characterController.velocity.z
                    ).magnitude;

                    // Set the Speed Float for the Idle/Walk/Run blend tree
                    animator.SetFloat("Speed", horizontalVelocity); 
                    
                    // Set the IsGrounded Bool (for landing)
                    //animator.SetBool("IsGrounded", characterController.isGrounded);
                }
            }
        }
    }