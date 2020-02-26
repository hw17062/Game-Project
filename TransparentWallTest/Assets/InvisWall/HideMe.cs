using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code used from here: https://answers.unity.com/questions/316064/can-i-obscure-an-object-using-an-invisible-object.html

public class HideMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.material.renderQueue = 3002;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
