using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	public float maxShieldHealth;
	public float shieldHealthPerInterval;
	public float maxBrokenShieldTime;
	public Player player;

	private bool shieldIsUp = false;
	private float currentShieldHealth;
	private float currentBrokenShieldTime;
	private Transform body;
	private bool ex = false;
	
	void Start() {
		currentShieldHealth = maxShieldHealth;
		body = transform.Find ("Body");
	}

	void Update() {
		if(shieldIsUp){
			print ("ex: " + ex);
			if(!ex && currentShieldHealth > 0){
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
	
	public void ShieldUp(bool exAttempt) {
		ex = exAttempt && player.SpendEx(1);
		if(currentBrokenShieldTime <= 0 && !shieldIsUp){
			shieldIsUp = true;
		}
	}
	
	public bool ShieldIsBroken(){
		return(currentBrokenShieldTime > 0);
	}
	
	public void ShieldDown() {
		shieldIsUp = false;
		ex = false;
	}
	
	public bool IsShieldUp() {
		return(shieldIsUp);
	}
	
	public float CurrentHealthRatio(){
		return(currentShieldHealth / maxShieldHealth);
	}
	
}
