using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Engine : Structure {
	private DamageBehavior damageBehavior;
	private SpriteRenderer bodySprite;
	private GameObject player;
	private GameObject enemyPlayer;
	private int enemyPlayerNumber;
	
	void Start(){
		damageBehavior = GetComponent<DamageBehavior>();
		bodySprite = transform.Find ("Body").GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		if(enemyPlayer == null){
			enemyPlayer = GetComponent<Entity>().EnemyPlayer();
			enemyPlayerNumber = enemyPlayer.GetComponent<Entity>().affinity.GetComponent<Fleet>().playerNumber;
		}
		bodySprite.color = NormalColor();
	}
	
	public override void ReceiveHit(float damage, GameObject attackerObject, GameObject attack) {
		IAttacker attacker = ResolveAttacker(attackerObject);
		if(attacker != null) attacker.RegisterSuccessfulAttack(0);
		damageBehavior.ReceiveDamage(damage);
		
		if(damageBehavior.CurrentHealthRatio() <= 0){
			DestroyMe ();
		}
}
	
	private void DestroyMe(){
		Destroy (gameObject);
		StateController.lastWinner = enemyPlayerNumber;
		GameController.LoadWinScreen();
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
