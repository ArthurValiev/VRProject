using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2: MonoBehaviour
{

	private int layerMask = 1 << 8;

    public void Update()
    {
        RaycastHit hit;

        GameObject reticle = this.gameObject.transform.GetChild(0).gameObject;

        Vector3 raycastDir = reticle.transform.position - transform.position;

        Ray landingRay = new Ray(transform.position, raycastDir);

        if (Physics.Raycast(landingRay, out hit, 1000, layerMask))
        {
            Debug.DrawRay(transform.position, raycastDir * 1000, Color.white);
            Debug.Log(hit.transform.name);
        }
        else
        {
            Debug.DrawRay(transform.position, raycastDir * 1000, Color.red);
            Debug.Log(hit.transform.name);
        }


        //Debug.Log("HELLO");
    }
}
