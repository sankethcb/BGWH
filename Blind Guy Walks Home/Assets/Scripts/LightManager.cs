using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    GameObject target;

    public GameObject flareLight; //Get the flare in the inspector

    float cooldown = 15.0f;
    float next = 5.0f;

	// Use this for initialization
	void Start ()
    {
        target = GameObject.Find("Target");
	}
	
	// Update is called once per frame
	void Update ()
    {
        //After a certain amount of time, create a flare at the position of the target
		if (Time.time > next)
        {
            Instantiate(flareLight, target.transform.position, Quaternion.identity);
            next = Time.time + cooldown;
        }
	}
}
