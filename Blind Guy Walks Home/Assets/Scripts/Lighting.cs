using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    Light theLight;

    float activeTime = 10.0f; //How long before next flash (E.g. activeTime = 10 means it will keep the light up until 10 seconds before the next flash)
    float cooldown = 10.0f; //How long between the flashes
    float next = 5.0f; //The time that next flash happens

    bool isMax = false; //If the intensity is the highest or not

    float t = 0.0f; //Rate of interpolation

    // Use this for initialization
    void Start ()
    {
        theLight = gameObject.GetComponent<Light>();
        theLight.intensity = 0.0f; //Intensity starts at 0, leaving the player blind
    }
	
	// Update is called once per frame
	void Update ()
    {
        LightUp();
        Debug.Log("Time: " + Time.time);
        Debug.Log("Next: " + next);
    }

    // Flash a Point Light over a period of time
    void LightUp()
    {
        //If it is time for the next flash
        if (Time.time > next)
        {
            theLight.intensity = Mathf.Lerp(0.0f, 20.0f, t); //Lerp the intensity from 0 to 20
            t += 5.0f * Time.deltaTime;
            //If the intensity reaches maximum, start the cooldown
            if (theLight.intensity == 20.0f)
            {
                next = Time.time + cooldown;
                isMax = true;
                t = 0.0f;
            }
        }
        //If the active time is over
        if (next < Time.time + activeTime && isMax == true)
        {
            theLight.intensity = Mathf.Lerp(20.0f, 0.0f, t); //Lerp the intensity from 20 to 0
            t += 1.0f * Time.deltaTime;
            if (theLight.intensity == 0.0f)
            {
                isMax = false;
                t = 0.0f;
            }
        }
    }
}
