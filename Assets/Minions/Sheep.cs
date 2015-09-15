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
	
	public override void Start(){
		base.Start ();
		maxStartupTime = Random.Range (1, 5);
		vehicleControls = GetComponent<VehicleControls>();
		turretTransform = transform.Find ("Turret");
		turret = turretTransform.GetComponent<SheepTurret>();
		turret.owner = gameObject;
		if(reversePosition){
			OrientationHelper.RotateTransform(turretTransform, -90);
		}else{
			OrientationHelper.RotateTransform(turretTransform, 90);
		}
	}

	public override void Update () {
		base.Update ();
		currentStartupTime += Time.deltaTime;
		
		if(currentStartupTime < maxStartupTime){
			if(reversePosition){
				vehicleControls.Steer (-0.25f);
			}else{
				vehicleControls.Steer (0.25f);
			}
		}
		vehicleControls.Idle ();
		print(GetComponent<Rigidbody2D>().velocity);
		
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
			turret.CreateBullet ();
			timeSinceLastFire = 0f;
		}
	}
	
	protected void Upgrade(){
		skipDestroyOnMerge = true;
		damageBehavior.IncreaseMaxHealth(2);
		damageBehavior.HealToFull();
		firesPerSecond += 1;
		transform.localScale += new Vector3(0.1f, 0.1f, 0);
		GetComponent<Rigidbody2D>().mass += 100;
	}
}
