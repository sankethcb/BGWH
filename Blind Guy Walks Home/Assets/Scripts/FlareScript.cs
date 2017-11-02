using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareScript : MonoBehaviour
{
    Light flareLight;
    Rigidbody flareRgbd;

    bool maxIntensity = true;
    float t = 0.0f; //Rate of interpolation

    // Use this for initialization
    void Start ()
    {
        flareLight = gameObject.GetComponent<Light>();
        flareRgbd = gameObject.GetComponent<Rigidbody>();

        //Shoots the flare at a random direction
        Vector3 direction = new Vector3(Random.Range(-500.0f, 500.0f), 1000.0f, Random.Range(-1000.0f, 0.0f));
        flareRgbd.AddForce(direction);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (flareLight.intensity == 10.0f || flareLight.intensity == 0.0f)
        {
            IntensityCheck();
            t = 0.0f;
        }
        if (maxIntensity == true)
        {
            t += 1.0f * Time.deltaTime;
            flareLight.intensity = Mathf.Lerp(10.0f, 0.0f, t);
        }
        else if (maxIntensity == false)
        {
            t += 1.0f * Time.deltaTime;
            flareLight.intensity = Mathf.Lerp(0.0f, 10.0f, t);
        }
        Destroy(gameObject, 5.0f); //Destroy the flare after 5 seconds
    }

    // Check if the intensity is at max or min
    void IntensityCheck()
    {
        if (flareLight.intensity == 10)
        {
            maxIntensity = true;
        }
        else if (flareLight.intensity == 0)
        {
            maxIntensity = false;
        }
    }
}
