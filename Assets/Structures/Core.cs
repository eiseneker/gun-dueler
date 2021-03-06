﻿using UnityEngine;
using System.Collections;

public class Core : Structure, IProjectilePassable {

	public int enemyPlayerNumber = 0;

	private DamageBehavior damageBehavior;
	private Fleet fleet;
	private CoreSentry coreSentry;

	void Start () {
		fleet = GetComponent<Entity>().affinity.GetComponent<Fleet>();
		damageBehavior = GetComponent<DamageBehavior>();
		transform.Find ("Sentry").GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
		transform.Find ("Body").GetComponent<SpriteRenderer>().color = fleet.teamColor;
		coreSentry = transform.Find ("Sentry").GetComponent<CoreSentry>();
		coreSentry.core = this;
	}
	
	void Update(){
		if(enemyPlayerNumber == 0){
			FetchEnemyPlayer();
		}
	}
	
	public override void ReceiveHit(float damage, GameObject attackerObject, GameObject attack) {
		IAttacker attacker = ResolveAttacker(attackerObject);
		if(attacker != null) {
			coreSentry.Fire (attackerObject);
			attacker.RegisterSuccessfulAttack(0);
		}
		damageBehavior.ReceiveDamage(0);
		fleet.ReceiveDamage(damage);
		if(fleet.CurrentHealthRatio() <= 0){
			DestroyMe ();
		}
	}
	
	public float CurrentHealthRatio(){
		return(damageBehavior.CurrentHealthRatio());
	}
	
	private void DestroyMe(){
		Destroy (gameObject);
		StateController.lastWinner = enemyPlayerNumber;
		GameController.LoadWinScreen();
	}
	
	private void FetchEnemyPlayer(){
		GameObject enemyPlayer = GetComponent<Entity>().EnemyPlayer();
		if(enemyPlayer){
			enemyPlayerNumber = enemyPlayer.GetComponent<Entity>().affinity.GetComponent<Fleet>().playerNumber;
		}
	}
	
	public Vector3 Velocity(){
		return(truck.Velocity ());
	}
}

