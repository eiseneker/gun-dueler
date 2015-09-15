using UnityEngine;
using System.Collections;

public class VehicleControls : MonoBehaviour {

	private Rigidbody2D myRigidbody;
	
	public float accelerationFactor = 50;
	public float brakeFactor = 50;
	public float maxVelocity = 10;
	public float minVelocity = 3;
	public float idleVelocity = 5;
	public float idleFactor = 25;
	public float speed;
	
	void Start(){
		myRigidbody = GetComponent<Rigidbody2D>();
	}

	public void Steer(float movement){
		float velocityRange = maxVelocity - minVelocity;
		float adjustedVelocity = myRigidbody.velocity.magnitude;
		float velocityRatio = adjustedVelocity/velocityRange;
		
		transform.Translate(Vector3.left * movement * velocityRatio * Time.deltaTime * speed);
	}
	
	public void Idle(){
		if(myRigidbody.velocity.magnitude > idleVelocity){
			myRigidbody.AddRelativeForce (Vector3.up * -accelerationFactor * Time.deltaTime * speed);
		}else{
			myRigidbody.AddRelativeForce (Vector3.up * idleFactor * Time.deltaTime * speed);
			myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, idleVelocity);
		}
	}
	
	public void Brake(){
		if(myRigidbody.velocity.magnitude < minVelocity){
			myRigidbody.AddRelativeForce (Vector3.up * accelerationFactor * Time.deltaTime * speed);
			myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, minVelocity);
		}else{
			myRigidbody.AddRelativeForce (Vector3.up * brakeFactor * -1 * Time.deltaTime * speed);
		}
	}
	
	public void Accelerate(){
		myRigidbody.AddRelativeForce (Vector3.up * accelerationFactor * Time.deltaTime * speed);
		myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, maxVelocity);
	}
}
