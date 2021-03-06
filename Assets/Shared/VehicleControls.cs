﻿using UnityEngine;
using System.Collections;

public class VehicleControls : MonoBehaviour {

	public Rigidbody2D myRigidbody;
	public bool steering;
	public bool accelerating;
	
	public float accelerationFactor = 50;
	public float brakeFactor = 50;
	public float defaultMaxYVelocity = 4;
	public float defaultMaxXVelocity = 10;
	public float minVelocity = 3;
	public float idleVelocity = 5;
	public float idleFactor = 25;
	public float steerSpeed;
	public float speedMultiplier = 1;
	public float maxVelocityModifier = 1;
	
	private float currentChargeDelay;
	private float maxChargeDelay = 5;
	private float maxXVelocity;
	private float maxYVelocity;
	private float maxChargeDuration = 0.5f;
	private float currentChargeDuration;
	public float yMovement;
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
				yVelocity *= 2f;
			}else if(accelerating){
				xVelocity *= 2.5f;
			}
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
			float velocityRange = (maxXVelocity * maxVelocityModifier) - minVelocity;
			float adjustedVelocity = myRigidbody.velocity.x;
			float velocityRatio = adjustedVelocity/velocityRange;
			yMovement = movement;
			float yVelocity = Mathf.Clamp (steerSpeed * velocityRatio * speedMultiplier, 0.5f, maxYVelocity * maxVelocityModifier);
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
			if(myRigidbody.velocity.x < minVelocity){
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
			myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, maxXVelocity * maxVelocityModifier);
		}
	}
	
	public void Accelerate(){
		Accelerate(1);
	}
	
	public void GetToIdle(){
		myRigidbody.velocity = new Vector2(idleVelocity, 0);
	}
	
	public void Charge(){
		currentChargeDuration = 0;
		currentChargeDelay = 0;
	}
	
	public void ResetChargeDelay(){
		currentChargeDelay = maxChargeDelay;
	}
	
	public bool IsCharging(){
		return(currentChargeDuration <= maxChargeDuration);
	}
	
	public bool CanCharge(){
		return(currentChargeDelay >= maxChargeDelay);
	}
	
}
