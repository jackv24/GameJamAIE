using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTheNodes : MonoBehaviour {

    public float speed = 1.0F;
    public int currentNode = 0;
    Transform startMarker;
    Transform endMarker;

    public GameObject[] selectorArr;
    bool startTrip = false;


    void Start()
    {
        startMarker = selectorArr[currentNode].transform;
        currentNode++;
        endMarker = selectorArr[currentNode].transform;
    }
    void Update()
    {
        //remove stuff below for your run, and either put a new button in, or call MoveTrain in another script.
        if (Input.GetKeyDown(KeyCode.P))
        {
            startTrip = true;
        }
        if (startTrip)
            MoveTrain();

    }

    public void MoveTrain()
    {
        if ((selectorArr[currentNode] != null))
        {
            transform.position += (endMarker.position - transform.position).normalized * speed * Time.deltaTime;

            if(Vector3.Distance(transform.position, endMarker.position) <= speed * Time.deltaTime)
            {
                startMarker = endMarker;
                currentNode++;

                if (currentNode >= selectorArr.Length)
                    currentNode = 0;

                endMarker = selectorArr[currentNode].transform;
            }
        }
    }
}
