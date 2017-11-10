using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InFrontOfCamera : MonoBehaviour {

	// Use this for initialization
    public float distance;

 
 void Update()
    {
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance;

        /*Vector3 canvasPos = Camera.main.transform.forward;

        canvasPos.y = 0;
        canvasPos.x = 0;

        transform.forward = canvasPos;*/
    }
}
