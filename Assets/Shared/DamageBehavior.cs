using UnityEngine;
using System.Collections;

public class DamageBehavior : MonoBehaviour {
	public float maxHealth;

	private float maxDamageAnimationTimer = 0.05f;
	private float currentDamageAnimationTimer;
	private GameObject overlay;
	private float currentHealth;

	void Start () {
		currentDamageAnimationTimer = maxDamageAnimationTimer;
		currentHealth = maxHealth;
		overlay = transform.Find ("Body").Find ("Damage Overlay").gameObject;
		overlay.SetActive (false);
	}
	
	void Update () {
		currentDamageAnimationTimer += Time.deltaTime;
		if(currentDamageAnimationTimer < maxDamageAnimationTimer){
			overlay.SetActive (true);
		}else{
			overlay.SetActive (false);
		}
	}
	
	public void ReceiveDamage(float health){
		currentDamageAnimationTimer = 0;
		currentHealth -= health;
	}
	
	public float CurrentHealthRatio(){
		return(currentHealth / maxHealth);
	}
	
}
