using UnityEngine;
using System.Collections;

public class Sheep : Minion {

	private float currentStartupTime;
	private float maxStartupTime;
	private float firesPerSecond = 2f;
	private bool skipDestroyOnMerge = false;
	private Transform turretTransform;
	private SheepTurret turret;
	private VehicleControls vehicleControls;
	private Rigidbody2D myRigidBody;
	private float reverseFactor = 1;
	private float acceleration;
	private GameObject enemyPlayer;
	private DriveBehavior driveBehavior;
	
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
		
		enemyPlayer = GetComponent<Entity>().EnemyPlayer();
		
		if(enemyPlayer && Mathf.Abs(enemyPlayer.transform.position.x - transform.position.x) < 1){
			driveBehavior = DriveBehavior.Idle;
		}else if(enemyPlayer == null){
			driveBehavior = DriveBehavior.Idle;
		}else if (enemyPlayer.transform.position.x > transform.position.x){
			driveBehavior = DriveBehavior.Accelerate;
		}else{
			driveBehavior = DriveBehavior.Brake;
		}
	}

	public override void Update () {
		base.Update ();
		currentStartupTime += Time.deltaTime;
		
		if(currentStartupTime < maxStartupTime){
			vehicleControls.Steer (0.25f * reverseFactor);
		}
		
		if(driveBehavior == DriveBehavior.Idle){
			vehicleControls.Idle ();
		}else if (driveBehavior == DriveBehavior.Accelerate){
		
			vehicleControls.Accelerate (acceleration);
		}else{
			vehicleControls.Brake ();
		}
		
		float probability = firesPerSecond * Time.deltaTime;
		
		if(Random.value < probability){
			Fire ();
		}
	}
	
	void OnCollisionEnter2D (Collision2D collision){
		if(collision.gameObject.GetComponent<Entity>()){
			if(collision.gameObject.GetComponent<Entity>().affinity == GetComponent<Entity>().affinity){
				Sheep sheep = collision.gameObject.GetComponent<Sheep>();
				if(sheep){
					sheep.Upgrade();
					if(!skipDestroyOnMerge){
						Destroy (gameObject);
					}
				}
				skipDestroyOnMerge = false;
			}else{
				IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
				if(harmedObject != null){
					harmedObject.ReceiveHit(1, gameObject);
				}
				DestroyMe();
			}
		}
	}
	
	protected void Fire () {
		if(timeSinceLastFire >= fireDelay){
			turret.CreateBullet (myRigidBody.velocity.x * reverseFactor);
			timeSinceLastFire = 0f;
		}
	}
	
	protected void Upgrade(){
		skipDestroyOnMerge = true;
		damageBehavior.IncreaseMaxHealth(2);
		damageBehavior.HealToFull();
		firesPerSecond += 1;
		transform.localScale += new Vector3(0.1f, 0.1f, 0);
	}
}
