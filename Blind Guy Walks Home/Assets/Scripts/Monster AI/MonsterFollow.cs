using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFollow : VehicleMovement
{

    public float maxForce;
    float steptime = 0.5f;
    public GameObject target;
    Vector3 ultimateForce;
    AudioSource m_stepAudio;
    // Use this for initialization
    public override void Start()
    {
        
        m_stepAudio = GetComponent<AudioSource>();
        InvokeRepeating("StepAudio", 0.0f, 0.8f);
        base.Start();
        
    }

    public override void CalcSteeringForces()
    {

        ultimateForce = Vector3.zero;
        ultimateForce += Seek(target.transform.position);
        ultimateForce += StayinBounds();
        ultimateForce += Avoid(2f);

        Vector3.ClampMagnitude(ultimateForce, maxForce);
        if (steptime > 0)
        {
            ApplyForce(ultimateForce);
            steptime -= Time.deltaTime;
            
        }
        else
        {
            velocity = velocity.normalized;
            StartCoroutine("Step");
        }

       
    }

    IEnumerator Step()
    {
        //StepAudio();
        yield return new WaitForSeconds(0.1f);
        
        steptime = 0.5f;
       
    }

    void StepAudio()
    {
        m_stepAudio.Play();
    }
   
}
