using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	public float maxShieldHealth;
	public float shieldHealthPerInterval;
	public float maxBrokenShieldTime;

	private bool shieldIsUp = false;
	private float currentShieldHealth;
	private float currentBrokenShieldTime;
	private Transform body;
	
	void Start() {
		currentShieldHealth = maxShieldHealth;
		body = transform.Find ("Body");
	}

	void Update() {
		if(shieldIsUp){
			if(currentShieldHealth > 0){
				currentShieldHealth = Mathf.Clamp (currentShieldHealth - shieldHealthPerInterval, 0, maxShieldHealth);
			}
		}else{
			if(currentShieldHealth < maxShieldHealth){
				currentShieldHealth = Mathf.Clamp (currentShieldHealth + shieldHealthPerInterval, 0, maxShieldHealth);
			}
		}
		if(ShieldIsBroken ()){
			currentBrokenShieldTime = Mathf.Clamp (currentBrokenShieldTime - Time.deltaTime, 0, maxBrokenShieldTime);
		}
		if(currentShieldHealth <= 0){
			BreakShield ();
		}
		
		body.gameObject.SetActive (shieldIsUp);		
	}
	
	public void DamageShield(float damageAmount) {
		currentShieldHealth = Mathf.Clamp (currentShieldHealth - damageAmount, 0, maxShieldHealth);
	}
	
	public void BreakShield() {
		currentBrokenShieldTime = maxBrokenShieldTime;
		ShieldDown ();
	}
	
	public void ShieldUp() {
		if(currentBrokenShieldTime <= 0 && !shieldIsUp){
			shieldIsUp = true;
		}
	}
	
	public bool ShieldIsBroken(){
		return(currentBrokenShieldTime > 0);
	}
	
	public void ShieldDown() {
		shieldIsUp = false;
	}
	
	public bool IsShieldUp() {
		return(shieldIsUp);
	}
	
	
}
