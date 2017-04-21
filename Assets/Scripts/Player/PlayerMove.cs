using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float acceleration = 15.0f;

    [Space()]
    public float gravity = 9.8f;
    public float jumpForce = 10.0f;
    public int airJumpAmount = 1;
    private int airJumpsLeft;

    [Space()]
    [Tooltip("The time after leaving the ground in which the player is still allowed to jump.")]
    public float jumpStopDelay = 0.1f;
    private float jumpStopTime;

    private Vector2 inputVector;
    private Vector3 moveVector;
    private Vector3 impactVector;

    private Vector3 cameraForward;
    private Vector3 cameraRight;

    private CharacterController controller;
    private PlayerInput playerInput;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        //If no camera sets it's transform values, use player transform instead
        cameraForward = transform.forward;
        cameraRight = transform.right;
    }

    void Update()
    {
        if(controller)
        {
            if (GameManager.instance.gameRunning)
            {
                //Get directional input
                inputVector.x = Mathf.Lerp(
                    inputVector.x,
                    (playerInput.ControllerConnected ? playerInput.moveX : 0.0f) + (playerInput.controllerIndex < 1 ? Input.GetAxisRaw("Horizontal") : 0),
                    acceleration * Time.deltaTime);

                inputVector.y = Mathf.Lerp(
                    inputVector.y,
                    (playerInput.ControllerConnected ? playerInput.moveY : 0.0f) + (playerInput.controllerIndex < 1 ? Input.GetAxisRaw("Vertical") : 0),
                    acceleration * Time.deltaTime);

                if (inputVector.magnitude > 1)
                    inputVector.Normalize();

                //Reset to zero for additive calculations below
                moveVector.x = 0;
                moveVector.z = 0;

                //Calculate move vector based on camera direction
                moveVector += inputVector.y * moveSpeed * cameraForward;
                moveVector += inputVector.x * moveSpeed * cameraRight;

                if (controller.isGrounded)
                {
                    jumpStopTime = Time.time + jumpStopDelay;
                    airJumpsLeft = airJumpAmount;
                }

                if (((playerInput.ControllerConnected ? playerInput.jump.WasPressed : false) || (playerInput.controllerIndex < 1 ? Input.GetButtonDown("Jump") : false)))
                {
                    if (Time.time <= jumpStopTime)
                    {
                        jumpStopTime = 0;

                        moveVector.y = jumpForce;
                    }
                    else if (airJumpsLeft > 0)
                    {
                        airJumpsLeft--;

                        moveVector.y = jumpForce;
                    }
                }
            }

            //Apply gravity if the controller is not grounded
            if (!controller.isGrounded)
            {
                moveVector.y -= gravity * Time.deltaTime;

                if(impactVector.y > 0)
                    impactVector.y -= gravity * Time.deltaTime;
            }

            //Face forward
            transform.rotation = Quaternion.LookRotation(cameraForward, Vector3.up);

            //Finally, move controller
            controller.Move((moveVector + impactVector) * Time.deltaTime);

            if (controller.isGrounded)
                impactVector = Vector3.Lerp(impactVector, Vector3.zero, 0.1f);
        }
    }

    public void SetCameraValues(Vector3 forward, Vector3 right)
    {
        cameraForward = forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        cameraRight = right;
        cameraRight.y = 0;
        cameraRight.Normalize();
    }

    public void AddImpact(Vector3 direction, float force)
    {
        direction.Normalize();

        if (direction.y < 0)
            direction.y *= -1;

        impactVector = direction * force;
    }
}
