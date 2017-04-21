using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupItems : MonoBehaviour {

    public delegate void StandardEvent();
    public event StandardEvent OnPickup;

    public int score = 1;
    public float rotSpeed = 20;
    public bool DoYouWantBob = true;
    public float bobCentre = 1.5f;
    public float bobSpeed = 3;
    public float bobVarition = 40;
    public GameObject particles;
    GameObject particle;
    
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
        if (particles)
        {
            particle = ObjectPooler.GetPooledObject(particles);
            particle.transform.position = new Vector3(other.transform.position.x, other.transform.position.y+1, other.transform.position.z);
        }
        gameObject.SetActive(false);



        if (OnPickup != null)
            OnPickup();
    }
    void Update()
    {
        transform.Rotate(0, (rotSpeed * Time.deltaTime), 0);
        if (DoYouWantBob)
        {
            Vector3 pos = transform.position;
            pos.y = pos.y + Mathf.Sin(Time.time * bobSpeed) / bobVarition;
            transform.position = pos;
        }

  
    }
}
