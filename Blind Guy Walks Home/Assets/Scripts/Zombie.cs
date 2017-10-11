using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class Zombie : VehicleMovement {

    //zombie target and obstacles
    public GameObject zombieTarget;
    public List<GameObject> obstacles;
    //public variables to tune the movement
    public float seekWeight;
    public float maxForce;
    public float avoidWeight;
    public float safeDistance;
    public float wanderWeight;

    //the futture pos of the object
    public Vector3 futurePos;

    //materials for debug lines
    public Material forwardMat;
    public Material rightMat;
    public Material futurePosMat;
    public Material targetMat;

    Vector2 humanXZ;
    Vector2 zombieXZ;

    AudioSource myAudio;
    public AudioClip monsterWalking;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        //set first zombies target to a human
        zombieTarget = GameObject.Find("Player");
        myAudio = GetComponent<AudioSource>();
        StartCoroutine(Footsteps());
    }

    protected override void CalcSteeringForces()
    {

        zombieXZ = new Vector2(transform.position.x, transform.position.z);
        if(zombieTarget != null)
            humanXZ = new Vector2(zombieTarget.transform.position.x, zombieTarget.transform.position.z);

        Vector3 ultamateForce = Vector3.zero;
        //loop through all obstacles for avodiance
        for (int i = 0; i < obstacles.Count; i++)
        {
            ultamateForce += base.AvoidObstacle(obstacles[i], safeDistance) * avoidWeight;
        }
        // avoid edges, see human for comments
        /*
        if (base.isXout())
        {

            if (base.isZout())
            {
                ultamateForce += base.Seek(Vector3.zero) * seekWeight;
            }
            else
            {
                ultamateForce += base.Seek(new Vector3(0, 0, transform.position.z)) * seekWeight;
            }
        }
        else if (base.isZout())
            ultamateForce += base.Seek(new Vector3(transform.position.x, 0, 0)) * seekWeight;
        
    */

        //if the zombies target isnt null seek the closes target
        if (zombieTarget != null && zombieTarget.activeSelf == true)
        {
            //if the distance to the object is less that the objects future pos dist seek the human
            //if (Vector2.Distance(humanXZ, zombieXZ) < zombieTarget.GetComponent<VehicleMovement>().futurePosDist)
                ultamateForce += base.Seek(zombieTarget.transform.position) * seekWeight;
            //else //if the human is far away pursue the human
                //ultamateForce += base.Seek(zombieTarget.GetComponent<Human>().futurePos) * seekWeight;
        }
            
        else
            ultamateForce += base.wander() * wanderWeight;

        //set future pos
        futurePos = GetFuturePos();
        
        //clamp and apply forces
        Vector3.ClampMagnitude(ultamateForce, maxForce);
        base.ApplyForce(ultamateForce);
        //playFootstep();

    }

    //opengl debug lines
    void OnRenderObject()
    {
        /*
        if (showLines) {
            forwardMat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(transform.position);
            GL.Vertex(transform.forward + transform.position);
            GL.End();

            rightMat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(transform.position);
            GL.Vertex(transform.right + transform.position);
            GL.End();

            futurePosMat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(futurePos + (transform.right * .5f));
            GL.Vertex(futurePos - (transform.right * .5f));
            GL.Vertex(futurePos + (transform.forward * .5f));
            GL.Vertex(futurePos - (transform.forward * .5f));
            GL.End();

            if (zombieTarget != null && zombieTarget.activeSelf == true)
            {
                targetMat.SetPass(0);
                GL.Begin(GL.LINES);
                GL.Vertex(transform.position);
                GL.Vertex(zombieTarget.transform.position);
                GL.End();
            }
        }

    */
    }


    


    IEnumerator Footsteps()
    {
        myAudio.PlayOneShot(monsterWalking);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Footsteps());
    }


}
