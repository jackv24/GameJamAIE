using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTheNodes : MonoBehaviour {

    public float speed = 1.0F;
    public int currentNode = 0;
    Transform endMarker;

    Vector3 lastPos;

    public GameObject[] selectorArr;
    bool startTrip = false;
    public float angleBetween = 0.0F;
    

    void Start()
    {
        endMarker = selectorArr[currentNode].transform;

        if (GameManager.instance)
            GameManager.instance.OnGameStart += delegate { startTrip = true; };
    }
    void Update()
    {
        //remove stuff below for your run, and either put a new button in, or call MoveTrain in another script.
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            startTrip = true;
        }*/
        if (startTrip)
            MoveTrain();

    }

    public void MoveTrain()
    {
        if ((selectorArr[currentNode] != null))
        {
            transform.position += (endMarker.position - transform.position).normalized * speed * Time.deltaTime;

            Vector3 dir = transform.position - lastPos;
            lastPos = transform.position;
            dir.Normalize();

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), 0.25f);

            if (Vector3.Distance(transform.position, endMarker.position) <= speed * Time.deltaTime)
            {
                currentNode++;

                if (currentNode >= selectorArr.Length)
                    gameObject.SetActive(false);
                    //currentNode = 0;

                endMarker = selectorArr[currentNode].transform;
            }
        }
    }
}
