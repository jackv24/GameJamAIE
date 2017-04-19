using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 10.0f;

    [Space()]
    public float gravity = 9.8f;
    public float jumpForce = 10.0f;

    private Vector2 inputVector;
    private Vector3 moveVector;

    private Vector3 cameraForward;
    private Vector3 cameraRight;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        cameraForward = transform.forward;
        cameraRight = transform.right;
    }

    void Update()
    {
        if(controller)
        {
            //Get directional input
            inputVector.x = Input.GetAxisRaw("Horizontal");
            inputVector.y = Input.GetAxisRaw("Vertical");

            inputVector.Normalize();

            //Reset to zero for additive calculations below
            moveVector.x = 0;
            moveVector.z = 0;

            //Calculate move vector based on camera direction
            moveVector += inputVector.y * moveSpeed * cameraForward;
            moveVector += inputVector.x * moveSpeed * cameraRight;

            //Apply gravity if the controller is not grounded
            if(!controller.isGrounded)
                moveVector.y -= gravity * Time.deltaTime;

            if (Input.GetButtonDown("Jump") && controller.isGrounded)
                moveVector.y = jumpForce;

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
