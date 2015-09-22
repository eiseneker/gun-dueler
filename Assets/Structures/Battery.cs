using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Battery : Structure {
	public DamageBehavior damageBehavior;
	
	private SpriteRenderer bodySprite;
	private GameObject player;
	private GameObject enemyPlayer;
	private int enemyPlayerNumber;
	private Fleet fleet;
	private bool disabled = false;
	
	void Start(){
		damageBehavior = GetComponent<DamageBehavior>();
		bodySprite = transform.Find ("Body").GetComponent<SpriteRenderer>();
		fleet = GetComponent<Entity>().affinity.GetComponent<Fleet>();
	}
	
	void Update () {
		bodySprite.color = NormalColor();
	}
	
	public override void ReceiveHit(float damage, GameObject attackerObject, GameObject attack) {
		if(!disabled){
			IAttacker attacker = ResolveAttacker(attackerObject);
			if(attacker != null) attacker.RegisterSuccessfulAttack(0);
			damageBehavior.ReceiveDamage(damage);
			
			if(damageBehavior.CurrentHealthRatio() <= 0){
				DestroyMe ();
			}
		}
	}
	
	private void DestroyMe(){
		disabled = true;
		fleet.ReceiveDamage(5);
		GameObject explosion = Instantiate ( Resources.Load ("Explosion"), transform.position, Quaternion.identity) as GameObject;
		explosion.transform.localScale -= new Vector3(0.5f, 0.5f, 0);
		fleet.truck.vehicleControls.maxVelocityModifier *= .975f;
	}
	
	private Color NormalColor(){
		float healthRatio = damageBehavior.CurrentHealthRatio();
		Color color;
		if(healthRatio > 0){
			color = new Color(1, healthRatio, healthRatio);
		}else{
			color = new Color(0.3f, 0.3f, 0.3f);
		}
		return(color);
	}
}
