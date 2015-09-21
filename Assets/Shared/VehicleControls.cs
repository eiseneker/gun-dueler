using UnityEngine;
using System.Collections;

public class VehicleControls : MonoBehaviour {

	private Rigidbody2D myRigidbody;
	private bool steering;
	private bool accelerating;
	
	public float accelerationFactor = 50;
	public float brakeFactor = 50;
	public float defaultMaxYVelocity = 4;
	public float defaultMaxXVelocity = 10;
	public float minVelocity = 3;
	public float idleVelocity = 5;
	public float idleFactor = 25;
	public float steerSpeed;
	public float speedMultiplier = 1;
	
	private float currentChargeDelay;
	private float maxChargeDelay = 5;
	private float maxXVelocity;
	private float maxYVelocity;
	private float maxChargeDuration = 0.5f;
	private float currentChargeDuration;
	private float yMovement;
	private Vector2 chargingXVector;
	private bool lockedControls = false;
	private Vector2 capturedChargeVelocity;
	private bool capturedChargeVelocityValid = false;
	
	void Start(){
		myRigidbody = GetComponent<Rigidbody2D>();
		maxXVelocity = defaultMaxXVelocity;
		currentChargeDuration = maxChargeDuration;
	}
	
	void Update(){
		currentChargeDelay += Time.deltaTime;
		currentChargeDuration += Time.deltaTime;
		if(currentChargeDuration <= maxChargeDuration){
			LockControls();
			CaptureChargeVelocity();
			print ("forcing velocity to " +capturedChargeVelocity );
			myRigidbody.velocity = capturedChargeVelocity;
		}else{
			ClearChargeVelocity();
			UnlockControls();
			maxXVelocity = defaultMaxXVelocity;	
			maxYVelocity = defaultMaxYVelocity;
		}
	}
	
	private void CaptureChargeVelocity(){
		if(!capturedChargeVelocityValid){
			float yVelocity = myRigidbody.velocity.y;
			float xVelocity = myRigidbody.velocity.x;
			if(steering){
				yVelocity *= 3f;
			}
			if(accelerating){
				xVelocity *= 1.5f;
			}
			print ("memoizing " + xVelocity + " , " + yVelocity);
			capturedChargeVelocity = new Vector2(xVelocity, yVelocity);
			capturedChargeVelocityValid = true;
		}
	}
	
	
	private void ClearChargeVelocity(){
		capturedChargeVelocityValid = false;
	}
	
	private void LockControls(){
		lockedControls = true;
	}
	
	private void UnlockControls(){
		lockedControls = false;
	}
	
	private void ForceChargeVelocity(){
		
	}

	public void Steer(float movement){
		if(!lockedControls){
			float velocityRange = maxXVelocity - minVelocity;
			float adjustedVelocity = myRigidbody.velocity.magnitude;
			float velocityRatio = adjustedVelocity/velocityRange;
			yMovement = movement;
			float yVelocity = Mathf.Clamp (steerSpeed * velocityRatio * speedMultiplier, 0, maxYVelocity);
			myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, yVelocity * yMovement);
			
			steering = true;
		}
	}
	
	public void Straight(){
		if(!lockedControls){
			if(steering){
				myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0);
				steering = false;
			}
		}
	}
	
	public void Idle(){
		if(!lockedControls){
			accelerating = false;
			if(myRigidbody.velocity.magnitude > idleVelocity){
				myRigidbody.AddRelativeForce (Vector3.up * -accelerationFactor * Time.deltaTime);
			}else{
				myRigidbody.AddRelativeForce (Vector3.up * idleFactor * Time.deltaTime);
				myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, idleVelocity);
			}
		}
	}
	
	public void Brake(){
		if(!lockedControls){
		accelerating = false;
			if(myRigidbody.velocity.magnitude < minVelocity){
				myRigidbody.AddRelativeForce (Vector3.up * accelerationFactor * Time.deltaTime * speedMultiplier);
				myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, minVelocity);
			}else{
				myRigidbody.AddRelativeForce (Vector3.up * brakeFactor * -1 * Time.deltaTime);
			}
		}
	}
	
	public void Accelerate(float factor){
		if(!lockedControls){
			accelerating = true;
			float accelerationOffset = (accelerationFactor - idleFactor) * factor;
			myRigidbody.AddRelativeForce (Vector3.up * (idleFactor + accelerationOffset) * Time.deltaTime * speedMultiplier);
			myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, maxXVelocity);
		}
	}
	
	public void Accelerate(){
		Accelerate(1);
	}
	
	public void GetToIdle(){
		myRigidbody.velocity = new Vector2(idleVelocity, 0);
	}
	
	public void Charge(){
		if(currentChargeDelay >= maxChargeDelay){
			currentChargeDuration = 0;
			currentChargeDelay = 0;
		}
	}
	
	public bool IsCharging(){
		return(currentChargeDuration <= maxChargeDuration);
	}
}
