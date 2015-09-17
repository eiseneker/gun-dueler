using UnityEngine;
using System.Collections;

public class Chaingun : Weapon {
	private float defaultSpeed = 20;
	private float defaultFireDelay = 0.02f;
	private int defaultMaxBulletsInPlay = 100;
	
	private GameObject bulletPrefab;
	private float speed;
	private AudioClip soundClip;
	private float currentReloadInterval;
	private float maxReloadInterval = 0.2f;
	private GameObject ammoMeter;
	
	
	public Chaingun(){
		maxBulletsInPlay = defaultMaxBulletsInPlay;
		fireDelay = defaultFireDelay;
		speed = defaultSpeed;
		bulletPrefab = Resources.Load ("bullet") as GameObject;
		soundClip = Resources.Load<AudioClip>("Vulcan");
	}
	
	protected override void Update(){
		base.Update ();
		if(currentReloadInterval >= maxReloadInterval){
			if(currentAmmoCount < MaxAmmoCount()){
				currentAmmoCount = Mathf.Clamp (currentAmmoCount + Mathf.CeilToInt(Mathf.Pow(timeSinceLastFire, 2)), 0, MaxAmmoCount());
			}
			currentReloadInterval = 0;
		}
		if(ammoMeter && CurrentAmmoRatio() == 1){
			Destroy (ammoMeter);
		}
		currentReloadInterval += Time.deltaTime;
	}
 
	public void Fire (bool exAttempt) {
		if(CanFire ()){
			if(ammoMeter == null){
				ammoMeter = Instantiate ( Resources.Load ("HUD/Ammo Meter"), transform.position, Quaternion.identity) as GameObject;
				ammoMeter.GetComponent<AmmoMeter>().player = player;
				ammoMeter.GetComponent<AmmoMeter>().weapon = this;
			}
			bool ex = exAttempt;
			
			speed = defaultSpeed;
			fireDelay = defaultFireDelay;
			maxBulletsInPlay = defaultMaxBulletsInPlay;
			
			if(ex){
				maxBulletsInPlay *= 2;
				fireDelay /= 1.2f;
				speed *= 1.2f;
			}
			
			//TODO: Find alternative
//			if(player.yMovement < 0){
//				speed *= 1.5f;
//			}
			
			if(ex){
				CreateBullet (0.2f);
				CreateBullet (-0.2f);
			}else{
				CreateBullet (0.1f);
				CreateBullet (-0.1f);
			}
			timeSinceLastFire = 0f;
		}
	}
	
	private BulletProjectile newProjectile(){
		GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
		BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
		return(bullet);
	}
	
	private void CreateBullet(float xOffset){
		if(currentAmmoCount > 0){
			AudioSource.PlayClipAtPoint(soundClip, transform.position);
			BulletProjectile bullet = newProjectile();
			bullet.speed = speed;
			bullet.weapon = this;
			bullet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
			float xMovement = player.GetComponent<Rigidbody2D>().velocity.magnitude;
			bullet.yVector = 1;
			bullet.xVector = Mathf.Round (xMovement) + Random.Range(-1f, 1f);
			bullet.transform.position = new Vector3((Mathf.Round(transform.position.x * 100f) / 100f) + xOffset, transform.position.y, 0);
			RegisterBullet ();
			OrientProjectile(bullet);
			currentAmmoCount--;
		}
	}
	
	protected override int MaxAmmoCount(){
		return(100);
	}
	
}
