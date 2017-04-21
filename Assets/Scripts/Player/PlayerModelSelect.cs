using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelSelect : MonoBehaviour
{
    public static int lastModelIndex = 0;

    public GameObject[] modelChildren;

    void Start()
    {
        if(modelChildren.Length > 0)
        {
            foreach (GameObject obj in modelChildren)
                obj.SetActive(false);

            modelChildren[lastModelIndex].SetActive(true);
            lastModelIndex++;

            if (lastModelIndex >= modelChildren.Length)
                lastModelIndex = 0;
        }
    }
}
