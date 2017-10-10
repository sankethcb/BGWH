using UnityEngine;
using System.Collections;

public abstract class VehicleMovement : MonoBehaviour {

    //a vec for the direction of the scene
    Vector3 direction;
    //a vec for the velocity of the object
    Vector3 velocity;
    //a vector for acceleration
    Vector3 acceleration;
    
    //Public floats to tune the movement
    public float mass;
    public float maxSpeed;
    public float radius;
    //float to set how far the future position is placed
    public float futurePosDist;

    //a float to set the bounds of the park
    float parkheight;

    //bool to change the size of the lines
    public bool showLines;


    // Update is called once per frame
    void Update () {

        drawLines();
        CalcSteeringForces();
        updatePosition();
        setTranform();
    }

    protected abstract void CalcSteeringForces();

    public virtual void Start()
    {
        //defalut park height of 30
        parkheight = 30;
        //start with debug lines on
        
    }

    void updatePosition()
    {
        

        // Step 1: Add acceleration vector  * time to velocity
        velocity += acceleration * Time.deltaTime;

        velocity.y = 0;

        // Step 2: Derive direction from velocity
        direction = velocity.normalized;

        transform.forward = direction;



        // Step 3: Reset acceleration
        acceleration = Vector3.zero;  // (new Vector3(0, 0, 0)  *0
    }

    //set the objects position via character controler 
    void setTranform()
    {
        //move the object
        gameObject.GetComponent<CharacterController>().Move(velocity * Time.deltaTime);
        //set the gameobjects y position to zero
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
    }

    //apply a force to the object divided by the mass
    protected void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    //a method to seek or pursue an enemy 
    protected Vector3 Seek(Vector3 targetPosition)
    {
        Vector3 dir = targetPosition - transform.position;
        
        //normalize & multiply by max speed to set velocity
        dir = dir.normalized * maxSpeed;

        //set desired velocity minus current velocity
        dir = dir - velocity;
        
        //return steering force
        return dir;
    }
    

    //A mehtod to flee or evade an enemy
    protected Vector3 Flee(Vector3 targetPosition)
    {
        //desired velocity is equal to the current postion minus the target postion
        Vector3 desiredVelocity = transform.position - targetPosition;

        //normalize velocity and multiply it by the max speed
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        //
        Vector3 steeringForce = desiredVelocity + velocity;

        return steeringForce;
    }

    //a Method that returns the future postion of the object at a certian time
    protected Vector3 GetFuturePos()
    {
        //takes the forward vector * the future pos distance and adds the current position
        return transform.forward * futurePosDist + transform.position;
    }


    /// <summary>
    /// a method that checks if the x value is out of bounds
    /// </summary>
    /// <returns>returns true if it is out of bounds</returns>
    protected bool isXout()
    {
        if (transform.position.x > parkheight ||transform.position.x < -parkheight)
            return true;
        return false;
    }

    /// <summary>
    /// a Method thet checks if the z value is out of bounds
    /// </summary>
    /// <returns>Returns true if it is out ouf bounds</returns>
    protected bool isZout()
    {
        if (transform.position.z > parkheight ||transform.position.z < -parkheight)
            return true;
        return false;
    }

    /// <summary>
    /// A method that causes the object to wander arround the screen
    /// </summary>
    /// <returns>Returns a vector that causes the character to move</returns>
    protected Vector3 wander()
    {
        //set the future pos to the future Pos Dist
        Vector3 futurePos = direction * futurePosDist;
        float angle = Random.Range(0, 360);
         
        
        //get a random vector 3 that rotates arround in a circle
        Vector3 rotation = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle)) * 50;
        futurePos = futurePos + rotation;
        futurePos = futurePos.normalized;
        futurePos *= maxSpeed;
        return futurePos;
        
    }
    /// <summary>
    /// A method that causes the vehicle to steer away from the current position
    /// </summary>
    /// <param name="obstacle">The obeject that the vehicle is steering away from </param>
    /// <param name="safeDistance">The distance at which the object will start trying to avoid</param>
    /// <returns>Returns a vector3 steering force</returns>
    protected Vector3 AvoidObstacle(GameObject obstacle, float safeDistance)
    {
        //get a vector from the vehicle to the obstacle
        Vector3 distToObs = obstacle.transform.position - transform.position;
        
        //if the dot product of your vector and the object is negitive the object is behind you, so return zero
        if(Vector3.Dot(distToObs, transform.forward) > 0)
        {
            
            //if the object is too far to the right or left return zero
            if (Mathf.Abs(Vector3.Dot(distToObs, transform.right)) < radius)
            {
                
                //if the object is to far away return zero
                if(distToObs.magnitude < safeDistance)
                {
                    
                    //if the object is to the right move to the right if the obect is on the left move left
                    if (Vector3.Dot(distToObs, transform.right) >= 0)
                    {
                        //set the steering force to the left of the vechicle    
                        Vector3 futurePos = -transform.right;
                        futurePos = futurePos.normalized;
                        futurePos *= maxSpeed;
                        return futurePos; 
                    }
                    else
                    {
                        //set the steering force to the right of the vechicle    
                        Vector3 futurePos = transform.right;
                        futurePos = futurePos.normalized;
                        futurePos *= maxSpeed;
                        return futurePos;
                    }
                }
                
            }
        }
        //if all else fails return a zeo vector
        return Vector3.zero;
    }

    /// <summary>
    /// Method that changes the value of showlines when d is pressed
    /// </summary>
    public void drawLines()
    {
        if (Input.GetKeyDown(KeyCode.D))
            showLines = !showLines;
    }
}
