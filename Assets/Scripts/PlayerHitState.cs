using UnityEngine;
using System.Collections;

public class PlayerHitState : MonoBehaviour {
	private float maxInvincibleTime = 2f;
	private float maxCriticalTime = 2f;
	private Transform body;
	private SpriteRenderer bodySprite;
	private HitState currentHitState;
	private PlayerHitState playerHitState;
	private float currentHitTime = 0f;
	private GameObject player;
	
	private enum HitState
	{
		NotHit,
		Invincible,
		Critical
	}

	public PlayerHitState(GameObject inputPlayer){
		player = inputPlayer;
		body = player.transform.Find ("Body");
		bodySprite = body.GetComponent<SpriteRenderer>();
	}

	public void RefreshHitState(){
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
		}else if(currentHitState == HitState.Critical){
			Destroy (player);
		}
	}
	
	private void AdvanceHitTime(){
		currentHitTime += Time.deltaTime;
		if(IsInvincible() && HasInvincibleTimeExpired()){
			SwitchToCritical();
		}
	}
	
	private void SwitchToNotHit() {
		UpdateColor (Color.white);		
		currentHitTime = 0f;
		currentHitState = HitState.NotHit;
	}
	
	private void SwitchToInvincible() {
		UpdateColor (Color.blue);		
		currentHitState = HitState.Invincible;
	}
	
	private void SwitchToCritical() {
		UpdateColor (Color.green);			
		currentHitState = HitState.Critical;
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
	
	private void UpdateColor(Color color){
		bodySprite.color = color;
	}
}
