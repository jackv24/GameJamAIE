using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{
    public GameObject explosionPrefab;

    [Space()]
    public float bounceDetonateDelay = 1.0f;

    void OnCollisionEnter()
    {
        StartCoroutine("ExplodeWithDelay");
    }

    IEnumerator ExplodeWithDelay()
    {
        yield return new WaitForSeconds(bounceDetonateDelay);

        if (explosionPrefab)
        {
            GameObject obj = ObjectPooler.GetPooledObject(explosionPrefab);
            obj.transform.position = transform.position;
        }

        Rigidbody body = GetComponent<Rigidbody>();
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;

        gameObject.SetActive(false);
    }
}
