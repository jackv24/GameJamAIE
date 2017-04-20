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

    [Space()]
    [Tooltip("The time after leaving the ground in which the player is still allowed to jump.")]
    public float jumpStopDelay = 0.1f;
    private float jumpStopTime;

    private Vector2 inputVector;
    private Vector3 moveVector;

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
            //Get directional input
            inputVector.x = Mathf.Lerp(
                inputVector.x,
                playerInput.moveX + (playerInput.controllerIndex < 1 ? Input.GetAxisRaw("Horizontal") : 0),
                acceleration * Time.deltaTime);

            inputVector.y = Mathf.Lerp(
                inputVector.y,
                playerInput.moveY + (playerInput.controllerIndex < 1 ? Input.GetAxisRaw("Vertical") : 0),
                acceleration * Time.deltaTime);

            if(inputVector.magnitude > 1)
                inputVector.Normalize();

            //Reset to zero for additive calculations below
            moveVector.x = 0;
            moveVector.z = 0;

            //Calculate move vector based on camera direction
            moveVector += inputVector.y * moveSpeed * cameraForward;
            moveVector += inputVector.x * moveSpeed * cameraRight;

            if (controller.isGrounded)
                jumpStopTime = Time.time + jumpStopDelay;

            //Apply gravity if the controller is not grounded
            if(!controller.isGrounded)
                moveVector.y -= gravity * Time.deltaTime;

            if ((playerInput.jump.WasPressed || Input.GetButtonDown("Jump")) && Time.time <= jumpStopTime)
            {
                jumpStopTime = 0;

                moveVector.y = jumpForce;
            }

            //Finally, move controller
            controller.Move(moveVector * Time.deltaTime);
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
}
