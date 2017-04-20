using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInitialTorque : MonoBehaviour
{
    public Vector3 minTorque;
    public Vector3 maxTorque;

    private Rigidbody body;

    void OnEnable()
    {
        body = GetComponent<Rigidbody>();

        if (body)
        {
            Vector3 torque = new Vector3
                (
                    Random.Range(minTorque.x, maxTorque.x),
                    Random.Range(minTorque.y, maxTorque.y),
                    Random.Range(minTorque.z, maxTorque.z)
                );

            body.AddTorque(torque, ForceMode.Impulse);
        }
    }
}
