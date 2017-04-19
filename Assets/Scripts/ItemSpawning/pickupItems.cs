using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupItems : MonoBehaviour {

    public int score;
    public float rotSpeed;
    public bool DoYouWantBob;
    public float bobCentre;
    public float bobSpeed;
    
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
