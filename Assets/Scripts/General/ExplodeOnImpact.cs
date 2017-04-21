using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{
    public GameObject explosionPrefab;

    [Space()]
    public float bounceDetonateDelay = 1.0f;

    [Space()]
    public float explosionRadius = 5.0f;
    public float explosionForce = 10.0f;

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

        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach(Collider col in cols)
        {
            PlayerMove move = col.GetComponent<PlayerMove>();

            if (move)
            {
                Vector3 dir = col.transform.position - transform.position;
                dir.y *= 2;
                dir.x /= 2;
                dir.Normalize();

                move.AddImpact(dir, explosionForce);
            }
        }

        gameObject.SetActive(false);
    }
}
