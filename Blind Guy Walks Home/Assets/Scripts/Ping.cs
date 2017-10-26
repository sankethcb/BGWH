using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : MonoBehaviour {

   // public AudioClip m_pingAudio;
    public  GameObject m_pingObject;
    RaycastHit[] hits;

    // Use this for initialization
    void Start ()
    {
       
    }
	
	
	void Update ()
    {


        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            SendPing();
        
       


	}

    void SendPing()
    {
        Debug.Log("1");
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
