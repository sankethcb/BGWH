using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : MonoBehaviour {

   
    public  GameObject m_pingObject;
    public GameObject m_pingAnimation;
    RaycastHit[] hits;

    // Use this for initialization
    void Start ()
    {
       
    }
	
	
	void Update ()
    {


        //if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        if (Input.GetKeyDown(KeyCode.Space))
            SendPing();
        
       


	}

    void SendPing()
    {
        GameObject pingAnimation = Instantiate(m_pingAnimation, transform.position, Quaternion.Euler(new Vector3(90,0,0)));
        Destroy(pingAnimation,0.7f);
        hits = Physics.SphereCastAll(transform.position, 10.0f, transform.forward);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "Obstacle")
            {
                GameObject pingInstance = Instantiate(m_pingObject, hit.collider.transform.position, Quaternion.identity);
                Destroy(pingInstance, 3);
                

            }
        }
    }


}
