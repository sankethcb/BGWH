using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// Sanketh Bhat
/// VehicleMovement Class
abstract public class VehicleMovement : MonoBehaviour
{
    public float radius;
    public float mass;
    public float maxSpeed;
    public float bounds;
    GameObject[] obstacles;

    Vector3 position;
	Vector3 direction;
	public Vector3 velocity;
	public Vector3 acceleration;
	Vector3 steeringForce;
	Vector3 desiredVelocity;
	Vector3 distance;
	Vector3 terrainCenter = Vector3.zero;
    Vector3 avoidForce;
  



    // Use this for initialization
    public virtual void Start ()
	{

        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
    }
	
	// Update is called once per frame
	public void Update ()
	{

		CalcSteeringForces ();

		UpdatePosition ();

		SetTransform ();

	}
    
	/// <summary>
	/// Updates the position of the agent
	/// </summary>
	void UpdatePosition ()
	{

		position = gameObject.transform.position;

		velocity += acceleration * Time.deltaTime;

		position += velocity * Time.deltaTime;

        direction = velocity.normalized;

		acceleration = Vector3.zero;
	}
	
	/// <summary>
	/// Calculates the steering forces
	/// abstract so has to be used in child classes
	/// </summary>
	public abstract void CalcSteeringForces ();


	/// <summary>
	/// Seeks the specified target
	/// </summary>
	/// <param name="targetPos">Target position.</param>
	/// Returns seeking steering force
	public Vector3 Seek (Vector3 targetPos)
	{
		desiredVelocity = targetPos - position;

		desiredVelocity.Normalize ();
		desiredVelocity *= maxSpeed;
		steeringForce = desiredVelocity - velocity;

		return steeringForce;
	}



	/// <summary>
	/// Flees the specified target
	/// </summary>
	/// <param name="targetPos">Target position.</param>
	/// Returns fleeing steering force
	public Vector3 Flee (Vector3 targetPos)
	{
		desiredVelocity = position - targetPos;
		
		desiredVelocity.Normalize ();
		desiredVelocity *= maxSpeed;
		steeringForce = desiredVelocity - velocity;

		return steeringForce;
	}


    public Vector3 Arrival(Vector3 targetPos)
    {
        desiredVelocity = targetPos - position;

        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        steeringForce = (desiredVelocity - velocity) * (targetPos - position).magnitude/10;

        return steeringForce;
    }


    /// <summary>
    /// Stays the bounds.
    /// </summary>
    /// <returns>The bounds.</returns>
    public Vector3 StayinBounds ()
	{
		if (position.x > 100    || position.x < -100 || position.z < -100 || position.z > 100 || position.y < -100 || position.y > 100)
            return Seek (terrainCenter);

		return Vector3.zero;
	}



	// <summary>
	/// Obstacle avoidance based on an agents minimum safe distance
	/// </summary>
	/// <param name="safeDist">Safe dist.</param>
	/// Returns avoidance steering force
	public Vector3 Avoid (float safeDist)
	{
		//Zeroing avoidance force which will be returned if the obstacle is not in the collision course 
		avoidForce = Vector3.zero;
		
		//Looping through a list of all obstacle objects
		foreach (GameObject obstacle in obstacles) {
			//Vector between obstacle and agent
			distance = obstacle.transform.position - position;
			//Checking if the obstacle is too far away
			if (distance.magnitude - (radius + 5f) < safeDist) {
				//Checking if the obstacle is in front
				if (Vector3.Dot (distance, transform.forward) >= 0) {
					//Checks if obstacle is in the avoidance zone
					if ((radius + 5f) > Vector3.Dot (distance, transform.right)) {
						//Checking for a positive or negative steering force from the right
						if (Vector3.Dot (distance, transform.right) >= 0)
							desiredVelocity = -transform.right * maxSpeed;
						else if (Vector3.Dot (distance, transform.right) < 0)
							desiredVelocity = transform.right * maxSpeed;
						
						avoidForce = desiredVelocity - velocity;
						//Weighting the avoidance steering force
						avoidForce *= (safeDist / distance.magnitude);
					}
				}
			}
		}
		return avoidForce;
	}


	/// <summary>
	/// Applies gravity to all flockers
	/// </summary>
	public void Gravity()
	{

		acceleration += Vector3.down * 9.81f;

	}
	/// <summary>
	/// Applies a force weighted by mass
	/// </summary>
	/// <param name="force">Force.</param>
	public void ApplyForce (Vector3 force)
	{
		acceleration += force / mass;
	}

	/// <summary>
	/// Sets the transform of the agent using the character controller
	/// </summary>
	void SetTransform ()
	{

			gameObject.transform.forward = direction;

		//Moving the agebnt along the velocity vector
		Debug.DrawLine (position, position + velocity, Color.red);
        gameObject.GetComponent<CharacterController>().Move(velocity * Time.deltaTime);
	}

    



}




