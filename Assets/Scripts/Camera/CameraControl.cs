using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    private PlayerMove move;
    private PlayerInput playerInput;

    [Space()]
    public float distance = 10.0f;
    public float zoomInSpeed = 20.0f;
    public float zoomOutSpeed = 5.0f;
    private float currentDistance;
    public LayerMask clippingLayer;

    [Space()]
    public float height = 1.5f;
    public float sensitivity = 150.0f;

    [Space()]
    public float pitch = 0.0f;
    public float pitchMin = -5.0f;
    public float pitchMax = 60.0f;

    void Start()
    {
        if (!target)
        {
            GameObject player = GameObject.FindWithTag("Player");

            if (player)
                target = player.transform;
        }

        if (target)
        {
            move = target.GetComponent<PlayerMove>();
            playerInput = target.GetComponent<PlayerInput>();
        }

        currentDistance = distance;
    }

    void LateUpdate()
    {
        if (target)
        {
            RaycastHit hitInfo;

            //Ensure camera does not clip through geometry
            if(Physics.Linecast(target.position, transform.position, out hitInfo, clippingLayer))
                currentDistance = Mathf.Lerp(currentDistance, hitInfo.distance, zoomInSpeed * Time.deltaTime);
            else
                currentDistance = Mathf.Lerp(currentDistance, distance, zoomOutSpeed * Time.deltaTime);

            //Move camera
            Vector2 input = new Vector2(
                (playerInput.ControllerConnected ? playerInput.cameraX : 0.0f) + (playerInput.controllerIndex < 1 ? Input.GetAxisRaw("Mouse X") : 0),
                (playerInput.ControllerConnected ? playerInput.cameraY : 0.0f) + (playerInput.controllerIndex < 1 ? Input.GetAxisRaw("Mouse Y") : 0)
                ) * sensitivity;

            //Move camera up/down
            pitch -= input.y * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

            //Move camera left/right
            transform.localEulerAngles = new Vector3(pitch, transform.localEulerAngles.y + input.x * Time.deltaTime, 0);
            transform.localEulerAngles = new Vector3(pitch, transform.localEulerAngles.y, 0);

            transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
            transform.position += transform.rotation * Vector3.back * currentDistance;
            transform.position += Vector3.up * height;

            //Update camera transform values on the player move script
            if(move)
                move.SetCameraValues(transform.forward, transform.right);
        }
    }
}