using UnityEngine;
using System.Collections;

public class PlayerHitState : MonoBehaviour {
	public GameObject player;

	private float maxInvincibleTime = 0.5f;
	private float maxCriticalTime = 1.5f;
	private HitState currentHitState;
	private PlayerHitState playerHitState;
	private float currentStateTime = 0f;
	private bool isHit = false;
	
	private enum HitState
	{
		Normal,
		Invincible,
		Critical
	}

	void Update(){
		if(isHit) {
			if(HasHitTimeExpired ()){
				SwitchToNotHit();
			}else{
				AdvanceStateTime();
			}
		}else if(currentHitState == HitState.Invincible){
			AdvanceStateTime();
		}
	}
	
	public void RegisterHit(){
		isHit = true;
		if(currentHitState == HitState.Normal) {
			SwitchToInvincible();
		}
	}
	
	public bool IsCritical(){
		return(currentHitState == HitState.Critical);
	}
	
	public void SwitchToInvincible() {
		currentHitState = HitState.Invincible;
		player.GetComponent<Animator>().SetBool("Invincible", true);
	}
	
	private void AdvanceStateTime(){
		currentStateTime += Time.deltaTime;
		if(IsInvincible() && HasInvincibleTimeExpired()){
			if(isHit){
				SwitchToCritical();
			}else{
				SwitchToNotHit();
			}
		}
	}
	
	private void SwitchToNotHit() {
		isHit = false;
		currentStateTime = 0f;
		currentHitState = HitState.Normal;
		player.GetComponent<Animator>().SetBool("Invincible", false);
		player.GetComponent<Animator>().SetBool("Critical", false);
	}
	
	private void SwitchToCritical() {
		currentHitState = HitState.Critical;
		player.GetComponent<Animator>().SetBool("Invincible", false);
		player.GetComponent<Animator>().SetBool("Critical", true);
	}
	
	private bool IsInvincible() {
		return(currentHitState == HitState.Invincible);
	}
	
	private bool HasInvincibleTimeExpired() {
		return(currentStateTime > maxInvincibleTime);
	}
	
	private bool HasHitTimeExpired() {
		return(currentStateTime > maxCriticalTime + maxCriticalTime);
	}
}
