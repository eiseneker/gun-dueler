using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnTurret : Agent {
	private float spawnsPerSecond = 0.075f;
	private int[] spawnLevelProbabilities = { 0, 0, 0, 1 };
	private bool disabled = false;
	private float[] spawnLevelInterval = { 1, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f };
	private DamageBehavior damageBehavior;
	private SpriteRenderer bodySprite;
	
	public GameObject[] minionPrefabs;
	public int level;
	
	void Start(){
		damageBehavior = GetComponent<DamageBehavior>();
		bodySprite = transform.Find ("Body").GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		if(GameController.gameStarted && !disabled){
			float probability = spawnsPerSecond * Time.deltaTime * spawnLevelInterval[level - 1];
			
			if(Random.value < probability){
				SpawnMinion ();
			}
		}
		bodySprite.color = NormalColor();
	}
	
	void SpawnMinion(){
		Quaternion rotation = new Quaternion();
		int level = spawnLevelProbabilities[Random.Range (0, spawnLevelProbabilities.Length)];
		GameObject minion = Instantiate (minionPrefabs[level], transform.position, rotation) as GameObject;
		minion.transform.parent = transform;
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
		float hurtRatio = damageBehavior.CurrentHealthRatio();
		return(new Color(1, hurtRatio, hurtRatio));
	}
}
