﻿using UnityEngine;
using System.Collections;

public class Sheep : Minion {

	private float currentStartupTime;
	private float maxStartupTime;
	private float firesPerSecond = 2f;
	private bool preserved = false;
	private Transform turretTransform;
	private SheepTurret turret;
	private VehicleControls vehicleControls;
	private Rigidbody2D myRigidBody;
	private float reverseFactor = 1;
	private float acceleration;
	private GameObject enemyPlayer;
	private DriveBehavior driveBehavior;
	private int upgradeCount;
	
	private enum DriveBehavior
	{
		Idle,
		Accelerate,
		Brake
	}
	
	public override void Start(){
		base.Start ();
		maxStartupTime = Random.Range (2, 4);
		vehicleControls = GetComponent<VehicleControls>();
		turretTransform = transform.Find ("Turret");
		turret = turretTransform.GetComponent<SheepTurret>();
		turret.owner = gameObject;
		myRigidBody = GetComponent<Rigidbody2D>();
		if(reversePosition){
			reverseFactor *= -1;
		}
		OrientationHelper.RotateTransform(turretTransform, 90 * reverseFactor);
		acceleration = Random.value;
		
		DetermineSpawnBehavior();
	}

	public override void Update () {
		base.Update ();
		currentStartupTime += Time.deltaTime;
		
		ManageDrivingBehavior();
		
		float probability = firesPerSecond * Time.deltaTime;
		
		if(Random.value < probability){
			Fire ();
		}
	}
	
	private void DetermineSpawnBehavior(){
		enemyPlayer = GetComponent<Entity>().EnemyPlayer();
		
		if(enemyPlayer && Mathf.Abs(enemyPlayer.transform.position.x - transform.position.x) < 2){
			driveBehavior = DriveBehavior.Idle;
		}else if(enemyPlayer == null){
			driveBehavior = DriveBehavior.Idle;
		}else if (enemyPlayer.transform.position.x > transform.position.x){
			driveBehavior = DriveBehavior.Accelerate;
		}else{
			driveBehavior = DriveBehavior.Brake;
		}
	}
	
	private void ManageDrivingBehavior(){
		if(currentStartupTime < maxStartupTime){
			vehicleControls.Steer (0.25f * reverseFactor);
		}else{
			vehicleControls.Straight ();
		}
		
		if(driveBehavior == DriveBehavior.Idle){
			vehicleControls.Idle ();
		}else if (driveBehavior == DriveBehavior.Accelerate){
			vehicleControls.Accelerate (acceleration);
		}else{
			vehicleControls.Brake ();
		}
	}
	
	void OnCollisionEnter2D (Collision2D collision){
		if(collision.gameObject.GetComponent<Entity>()){
			if(AffinitiesMatch(collision.gameObject)){
				Sheep sheep = collision.gameObject.GetComponent<Sheep>();
				if(sheep) Merge (sheep);
			}else{
				IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
				if(harmedObject != null){
					harmedObject.ReceiveHit(1, gameObject, gameObject);
				}
			}
		}
	}
	
	private bool AffinitiesMatch(GameObject collidedObject){
		return(collidedObject.GetComponent<Entity>().affinity == GetComponent<Entity>().affinity);
	}
	
	protected void Merge(Sheep sheep){
		if(this.upgradeCount + sheep.upgradeCount < 2){
			sheep.Upgrade(sheep.upgradeCount + 1);
			if(!preserved){
				Destroy (gameObject);
			}
		}
		preserved = false;
	}
	
	protected void Fire () {
		if(timeSinceLastFire >= fireDelay){
			turret.CreateBullet (myRigidBody.velocity.x * reverseFactor);
			timeSinceLastFire = 0f;
		}
	}
	
	protected void Upgrade(int count){
		for(int i = 0; i < count; i++){
			preserved = true;
			damageBehavior.IncreaseMaxHealth(2);
			damageBehavior.HealToFull();
			firesPerSecond += 1;
			transform.localScale += new Vector3(0.1f, 0.1f, 0);
			upgradeCount++;
		}
	}
}
