using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bombPrefab;

    [Space()]
    public GameObject groundReticlePrefab;
    private GameObject reticle;
    public float maxAimDistance = 10.0f;

    [Space()]
    public float fireAngle = 45.0f;

    public float fireDelay = 0.5f;
    private float nextFireTime = 0;

    [HideInInspector]
    public Camera aimCam;
    public LayerMask groundLayer;

    private bool shouldThrowBomb = false;

    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        reticle = (GameObject)Instantiate(groundReticlePrefab);
    }

    void LateUpdate()
    {
        if(reticle && aimCam && ((playerInput.ControllerConnected && playerInput.aim.IsPressed) || (playerInput.controllerIndex < 1 && Input.GetMouseButton(0))) && GameManager.instance.gameRunning)
        {
            reticle.SetActive(true);

            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireDelay;

                shouldThrowBomb = true;
            }

            RaycastHit hitInfo;

            Ray ray = aimCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (Physics.Raycast(ray, out hitInfo, 1000f, groundLayer))
            {
                if((hitInfo.point - transform.position).magnitude > maxAimDistance)
                {
                    //Ray cast from camera is too far, cast ray down from furthest point instead
                    Vector3 raycastPoint = (new Vector3(ray.direction.x, 0, ray.direction.z).normalized * maxAimDistance) + transform.position;
                    raycastPoint.y = 500f;

                    Physics.Raycast(raycastPoint, Vector3.down, out hitInfo, 1000f, groundLayer);
                }

                //Position and rotate reticle to match surface
                reticle.transform.position = hitInfo.point;
                reticle.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            }

            if (shouldThrowBomb)
            {
                shouldThrowBomb = false;

                //Spawn bomb at position
                GameObject bomb = ObjectPooler.GetPooledObject(bombPrefab);
                bomb.transform.position = transform.position + Vector3.up + transform.forward;

                Rigidbody body = bomb.GetComponent<Rigidbody>();

                //Calculate initial force to hit target
                Vector3 p = reticle.transform.position;

                float gravity = Physics.gravity.magnitude;
                float angle = fireAngle * Mathf.Deg2Rad;

                //Positions of target and bomb on same plane
                Vector3 planarTarget = new Vector3(p.x, 0, p.z);
                Vector3 planarPosition = new Vector3(bomb.transform.position.x, 0, bomb.transform.position.z);

                float distance = Vector3.Distance(planarTarget, planarPosition);
                float yOffset = bomb.transform.position.y - p.y;

                float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

                Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

                float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (bomb.transform.position.x > p.x ? -1 : 1);
                Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

                if(finalVelocity.magnitude != 0)
                    body.AddForce(finalVelocity, ForceMode.VelocityChange);
            }
        }
        else if (reticle.activeSelf)
        {
            reticle.SetActive(false);
        }
    }
}
