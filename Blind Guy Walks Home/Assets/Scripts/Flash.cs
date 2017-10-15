using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour {
    
    Image dark;
    Color c;

    float cooldown = 5.0f;
    float next = 5.0f;

    float min = 0.0f;
    float max = 1.0f;
    float t = 0.0f;

	// Use this for initialization
	void Start ()
    {
        dark = GetComponent<Image>();
        c = dark.color;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Time.time > next)
        {
            next += Time.time + cooldown;
            c.a = 0.0f;
            dark.color = c;
            t = 0.0f;
        }
        if (c.a < 1.0f)
        {
            c.a = Mathf.Lerp(min, max, t);
            t += 0.5f * Time.deltaTime;
            dark.color = c;
        }
    }
}
