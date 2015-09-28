using UnityEngine;
using System.Collections;

public class Neutral : Minion {

	private float currentStartupTime;
	private float firesPerSecond = 2f;
	private Transform turretTransform;
	private SheepTurret turret;
	private VehicleControls vehicleControls;
	private Rigidbody2D myRigidBody;
	private float reverseFactor = 1;
	private float acceleration;
	private int upgradeCount;
	private DriveBehavior driveBehavior;
	
	private enum DriveBehavior
	{
		None,
		Idle,
		Accelerate,
		Brake
	}
	
	public override void Start(){
		base.Start ();
		driveBehavior = DriveBehavior.None;
		
		vehicleControls = GetComponent<VehicleControls>();
		turretTransform = transform.Find ("Turret");
		turret = turretTransform.GetComponent<SheepTurret>();
		turret.owner = gameObject;
		myRigidBody = GetComponent<Rigidbody2D>();
		if(reversePosition){
			reverseFactor *= -1;
		}
		acceleration = Random.value;
		
		if(reversePosition){
			OrientationHelper.RotateTransform(turretTransform, 180);
		}
		foreach(Transform child in transform.Find ("Body").transform){
			CarWheel wheel = child.GetComponent<CarWheel>();
			if(wheel){
				wheel.vehicleControls = vehicleControls;
			}
		}
	}

	public override void Update () {
		base.Update ();
		ManageDrivingBehavior();
		currentStartupTime += Time.deltaTime;
		
		BePassive ();
		
		float probability = firesPerSecond * Time.deltaTime;
		
		if(Random.value < probability){
			Fire ();
		}
	}
	
	private void BePassive(){
		driveBehavior = DriveBehavior.Idle;
	}
	
	private void ManageDrivingBehavior(){
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
			IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
			if(harmedObject != null){
				harmedObject.ReceiveHit(1, gameObject, gameObject);
			}
		}
	}
	
	protected void Fire () {
//		if(timeSinceLastFire >= fireDelay){
//			turret.CreateBullet (myRigidBody.velocity.x * reverseFactor);
//			turret.CreateBullet (myRigidBody.velocity.x * reverseFactor * -1);
//			timeSinceLastFire = 0f;
//		}
	}
	
}
