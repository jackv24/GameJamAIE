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

    public Camera aimCam;
    public LayerMask groundLayer;

    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        reticle = (GameObject)Instantiate(groundReticlePrefab);
    }

    void LateUpdate()
    {
        if(reticle && aimCam && ((playerInput.ControllerConnected && playerInput.aim.IsPressed) || (playerInput.controllerIndex < 1 && Input.GetMouseButton(0))))
        {
            reticle.SetActive(true);

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
        }
        else if (reticle.activeSelf)
        {
            reticle.SetActive(false);
        }
    }
}
