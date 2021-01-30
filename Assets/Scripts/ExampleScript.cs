using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Loop());
        Debug.Log("Start finished");
    }

    public IEnumerator Loop()
    {
        int i = 0;
        while (i < 1000) {
            yield return StartCoroutine(InnerLoop());
            Debug.Log(i.ToString()+" before yield: "+Time.frameCount.ToString());
            yield return null; //wastes 1 frame
            Debug.Log(i.ToString() + " after yield: " + Time.frameCount.ToString());
            i++;
        }
        Debug.Log("Innerloop finished");
    }

    public IEnumerator InnerLoop()
    {
        Debug.Log("InnerLoop running");
        yield return null;
    }
}