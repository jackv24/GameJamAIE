using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    public float lifeTime = 5.0f;

    void OnEnable()
    {
        StartCoroutine("Disable");
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(lifeTime);

        gameObject.SetActive(false);
    }
}
