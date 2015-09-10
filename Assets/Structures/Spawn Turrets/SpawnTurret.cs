﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnTurret : Agent {
	private bool disabled = false;
	private DamageBehavior damageBehavior;
	private SpriteRenderer bodySprite;
	private float currentSpawnCooldown;
	private GameObject minionsObject;
	private GameObject player;
	private Fleet fleet;
	
	public GameObject minionPrefab;
	public float maxSpawnCooldown;
	
	void Start(){
		damageBehavior = GetComponent<DamageBehavior>();
		bodySprite = transform.Find ("Body").GetComponent<SpriteRenderer>();
		minionsObject = GameObject.Find ("Minions");
		fleet = GetComponent<Entity>().affinity.GetComponent<Fleet>();
	}
	
	void Update () {
		currentSpawnCooldown += Time.deltaTime;
		if(GameController.gameStarted && !disabled && transform.position.x > -5 && transform.position.x < 5){
			if(currentSpawnCooldown >= maxSpawnCooldown){
				if(fleet.player == null && fleet.PlayerCanRespawn()){
					GameObject player = fleet.AddPlayer();
					player.transform.position = transform.position;
					player.rigidbody2D.AddForce(new Vector2(0, 1));
				}else{
					SpawnMinion ();
				}
				currentSpawnCooldown = 0;
			}
		}
		bodySprite.color = NormalColor();
	}
	
	void SpawnMinion(){
		Quaternion rotation = new Quaternion();
		GameObject minion = Instantiate (minionPrefab, transform.position, rotation) as GameObject;
		minion.transform.parent = minionsObject.transform;
		minion.transform.Find ("Ship").GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
		if(GetComponent<Entity>().reversePosition){
			minion.transform.rotation = Quaternion.Euler(0f, 0f, 180);
		}
	}
	
	public override void ReceiveHit(float damage, GameObject attackerObject) {
		if(!disabled){
			IAttacker attacker = ResolveAttacker(attackerObject);
			if(attacker != null) attacker.RegisterSuccessfulAttack(0);
			damageBehavior.ReceiveDamage(damage);
			
			if(damageBehavior.CurrentHealthRatio() <= 0){
				disabled = true;
				if(attacker != null) attacker.RegisterSuccessfulDestroy(5);
				DestroyMe ();
			}
		}
	}
	
	private void DestroyMe(){
		GameObject explosion = Instantiate ( Resources.Load ("Explosion"), transform.position, Quaternion.identity) as GameObject;
		explosion.transform.localScale -= new Vector3(0.5f, 0.5f, 0);
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