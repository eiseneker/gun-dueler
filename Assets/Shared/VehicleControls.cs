using UnityEngine;
using System.Collections;

public class VehicleControls : MonoBehaviour {

	private Rigidbody2D myRigidbody;
	private bool drivingStraight;
	
	
	public float accelerationFactor = 50;
	public float brakeFactor = 50;
	public float maxVelocity = 10;
	public float minVelocity = 3;
	public float idleVelocity = 5;
	public float idleFactor = 25;
	public float steerSpeed;
	public float speedMultiplier = 1;
	
	void Start(){
		myRigidbody = GetComponent<Rigidbody2D>();
	}

	public void Steer(float movement){
		float velocityRange = maxVelocity - minVelocity;
		float adjustedVelocity = myRigidbody.velocity.magnitude;
		float velocityRatio = adjustedVelocity/velocityRange;
		
		myRigidbody.velocity = new Vector2(myRigidbody.velocity.x * .99f, movement * steerSpeed * velocityRatio * speedMultiplier);
		drivingStraight = false;
	}
	
	public void Straight(){
		if(!drivingStraight){
			myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0);
			drivingStraight = true;
		}
	}
	
	public void Idle(){
		if(myRigidbody.velocity.magnitude > idleVelocity){
			myRigidbody.AddRelativeForce (Vector3.up * -accelerationFactor * Time.deltaTime);
		}else{
			myRigidbody.AddRelativeForce (Vector3.up * idleFactor * Time.deltaTime);
			myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, idleVelocity);
		}
	}
	
	public void Brake(){
		if(myRigidbody.velocity.magnitude < minVelocity){
			myRigidbody.AddRelativeForce (Vector3.up * accelerationFactor * Time.deltaTime * speedMultiplier);
			myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, minVelocity);
		}else{
			myRigidbody.AddRelativeForce (Vector3.up * brakeFactor * -1 * Time.deltaTime);
		}
	}
	
	public void Accelerate(float factor){
		float accelerationOffset = (accelerationFactor - idleFactor) * factor;
		myRigidbody.AddRelativeForce (Vector3.up * (idleFactor + accelerationOffset) * Time.deltaTime * speedMultiplier);
		myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, maxVelocity);
	}
	
	public void Accelerate(){
		Accelerate(1);
	}
	
	public void GetToIdle(){
		myRigidbody.velocity = new Vector2(idleVelocity, 0);
	}
}
