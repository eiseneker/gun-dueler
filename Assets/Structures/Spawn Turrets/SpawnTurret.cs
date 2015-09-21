using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnTurret : Structure {
	private bool disabled = false;
	private DamageBehavior damageBehavior;
	private SpriteRenderer bodySprite;
	private float currentSpawnCooldown;
	private GameObject minionsObject;
	private GameObject player;
	private Fleet fleet;
	private GameObject enemyPlayer;
	private float maxSpawnCooldown;

	public GameObject minionPrefab;
	public float defaultMaxSpawnCooldown;
	public float maxDistanceToPlayer;
	public float minimumSpawnCooldown;
	
	void Start(){
		damageBehavior = GetComponent<DamageBehavior>();
		bodySprite = transform.Find ("Body").GetComponent<SpriteRenderer>();
		minionsObject = GameObject.Find ("Minions");
		fleet = GetComponent<Entity>().affinity.GetComponent<Fleet>();
	}
	
	void Update () {
		if(enemyPlayer == null){
			enemyPlayer = GetComponent<Entity>().EnemyPlayer();
		}
		if(enemyPlayer && !OutOfSpawnRange()){
			maxSpawnCooldown = Mathf.Clamp (DistanceToEnemyPlayer()/maxDistanceToPlayer * defaultMaxSpawnCooldown, minimumSpawnCooldown, defaultMaxSpawnCooldown);
		
			currentSpawnCooldown += Time.deltaTime;
			if(GameController.gameStarted && !disabled){
				if(currentSpawnCooldown >= maxSpawnCooldown){
					if(fleet.player == null && fleet.PlayerCanRespawn()){
						GameObject player = fleet.AddPlayer();
						player.transform.position = transform.position;
						player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1));
					}else{
						SpawnMinion ();
					}
					currentSpawnCooldown = 0;
				}
			}else{
				currentSpawnCooldown = 0;
			}
		}
		bodySprite.color = NormalColor();
	}
	
	void SpawnMinion(){
		Quaternion rotation = new Quaternion();
		GameObject minion = Instantiate (minionPrefab, transform.position, rotation) as GameObject;
		minion.transform.parent = minionsObject.transform;
		Transform ship = minion.transform.Find ("Ship");
		ship.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
		minion.transform.rotation = Quaternion.Euler(0f, 0f, -90);
		if(GetComponent<Entity>().reversePosition){
			ship.GetComponent<Minion>().reversePosition = true;
			minion.transform.rotation = Quaternion.Euler(0f, 0f, -90);
		}
		ship.GetComponent<Rigidbody2D>().velocity = truck.Velocity();
		Physics2D.IgnoreCollision(ship.GetComponent<Collider2D>(), GetComponent<Collider2D>());
	}
	
	public override void ReceiveHit(float damage, GameObject attackerObject, GameObject attack) {
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
		fleet.ReceiveDamage(5);
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
	
	private bool OutOfSpawnRange(){
		return(DistanceToEnemyPlayer() > maxDistanceToPlayer);
	}
	
	private float DistanceToEnemyPlayer(){
		return(Mathf.Abs (enemyPlayer.transform.position.x - transform.position.x));
	}
}
