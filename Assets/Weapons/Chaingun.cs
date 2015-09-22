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
	private float currentAngle;
	private float currentWindupTime;
	private float maxWindupTime = 1f;
	
	
	public Chaingun(){
		maxBulletsInPlay = defaultMaxBulletsInPlay;
		fireDelay = defaultFireDelay;
		speed = defaultSpeed;
		bulletPrefab = Resources.Load ("bullet") as GameObject;
		soundClip = Resources.Load<AudioClip>("Vulcan");
		currentAngle = 0;
	}
	
	protected override void Update(){
		base.Update ();
		OrientationHelper.RotateTransform(transform, currentAngle, 1);
	}
	
	public void Release(){
		currentWindupTime = Mathf.Clamp(currentWindupTime - Time.deltaTime, 0, maxWindupTime * 1.5f);
	}
 
	public void Fire (bool exAttempt, float angle) {
		currentAngle = angle;
		currentWindupTime = Mathf.Clamp(currentWindupTime + Time.deltaTime, 0, maxWindupTime * 1.5f);
		if(CanFire () && currentWindupTime >= maxWindupTime){
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
//				CreateBullet (0.2f);
//				CreateBullet (-0.2f);
			}else{
				foreach(Transform child in transform){
					CreateBullet (child.position);
				}
			}
			timeSinceLastFire = 0f;
		}
	}
	
	private BulletProjectile newProjectile(){
		GameObject bulletObject = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
		BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
		return(bullet);
	}
	
	private void CreateBullet(Vector3 origin){
		AudioSource.PlayClipAtPoint(soundClip, transform.position);
		BulletProjectile bullet = newProjectile();
		bullet.speed = speed;
		bullet.weapon = this;
		bullet.GetComponent<Entity>().affinity = player.GetComponent<Entity>().affinity;
		float bulletMagnitude = 1;
		bullet.yVector = bulletMagnitude;
		bullet.transform.position = origin;
		bullet.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity;
		RegisterBullet ();
	}
	
	protected override int MaxAmmoCount(){
		return(100);
	}
	
}
