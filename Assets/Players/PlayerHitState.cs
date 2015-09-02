using UnityEngine;
using System.Collections;

public class PlayerHitState : MonoBehaviour {
	public GameObject player;

	private float maxInvincibleTime = 0.5f;
	private float maxCriticalTime = 1.5f;
	private HitState currentHitState;
	private PlayerHitState playerHitState;
	private float currentHitTime = 0f;
	
	private enum HitState
	{
		NotHit,
		Invincible,
		Critical
	}

	void Update(){
		if(IsHit ()) {
			if(HasHitTimeExpired ()){
				SwitchToNotHit();
			}else{
				AdvanceHitTime();
			}
		}
	}
	
	public void RegisterHit(){
		if(currentHitState == HitState.NotHit) {
			SwitchToInvincible();
		}else if(IsCritical ()){
			Destroy (player);
		}
	}
	
	public bool IsCritical(){
		return(currentHitState == HitState.Critical);
	}
	
	private void AdvanceHitTime(){
		currentHitTime += Time.deltaTime;
		if(IsInvincible() && HasInvincibleTimeExpired()){
			SwitchToCritical();
		}
	}
	
	private void SwitchToNotHit() {
		currentHitTime = 0f;
		currentHitState = HitState.NotHit;
		player.GetComponent<Animator>().SetBool("Critical", false);
	}
	
	private void SwitchToInvincible() {
		currentHitState = HitState.Invincible;
		player.GetComponent<Animator>().SetBool("Invincible", true);
	}
	
	private void SwitchToCritical() {
		currentHitState = HitState.Critical;
		player.GetComponent<Animator>().SetBool("Invincible", false);
		player.GetComponent<Animator>().SetBool("Critical", true);
	}
	
	private bool IsHit() {
		return(currentHitState != HitState.NotHit);
	}
	
	private bool IsInvincible() {
		return(currentHitState == HitState.Invincible);
	}
	
	private bool HasInvincibleTimeExpired() {
		return(currentHitTime > maxInvincibleTime);
	}
	
	private bool HasHitTimeExpired() {
		return(currentHitTime > maxCriticalTime + maxCriticalTime);
	}
}
