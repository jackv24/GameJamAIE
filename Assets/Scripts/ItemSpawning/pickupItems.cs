using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupItems : MonoBehaviour {

    public int score = 1;
    public float rotSpeed = 20;
    public bool DoYouWantBob = true;
    public float bobCentre = 1.5f;
    public float bobSpeed = 3;
    
    float bobHeight;
    bool GoUp;
	// Use this for initialization
	void OnTriggerEnter(Collider other)
    {

        PlayerStats stats = other.GetComponent<PlayerStats>();

        if(stats)
        {
            stats.AddScore(score);
        }

       gameObject.SetActive(false);
    }
    void Update()
    {
        transform.Rotate(0, (rotSpeed * Time.deltaTime), 0);
        if (DoYouWantBob)
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.Sin(Time.time * bobSpeed) + bobCentre;
            transform.position = pos;
        }

  
    }
}
