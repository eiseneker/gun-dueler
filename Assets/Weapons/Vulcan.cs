using UnityEngine;
using System.Collections;

public class Vulcan : Weapon {
	private float defaultSpeed = 8;
	private float defaultFireDelay = 0.3f;
	private int defaultMaxBulletsInPlay = 8;
	
	private GameObject bulletPrefab;
	private float speed;
	private AudioClip soundClip;
	
	
	public Vulcan(){
		maxBulletsInPlay = defaultMaxBulletsInPlay;
		fireDelay = defaultFireDelay;
		speed = defaultSpeed;
		bulletPrefab = Resources.Load ("bullet") as GameObject;
		soundClip = Resources.Load<AudioClip>("Vulcan");
		print (soundClip);
	}
 
	public void Fire (bool exAttempt) {
		if(CanFire ()){
			AudioSource.PlayClipAtPoint(soundClip, transform.position);
			bool ex = exAttempt && player.SpendEx(1);
			
			speed = defaultSpeed;
			fireDelay = defaultFireDelay;
			maxBulletsInPlay = defaultMaxBulletsInPlay;
			
			if(ex){
				maxBulletsInPlay *= 2;
				fireDelay /= 1.5f;
				speed *= 1.5f;
			}
			
			BulletProjectile bullet = newProjectile();
		
			bullet.speed = speed;
			bullet.weapon = this;
			bullet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
			bullet.vector = Vector3.up;
			RegisterBullet ();
			OrientProjectile(bullet);
			timeSinceLastFire = 0f;
		}
	}
	
	private BulletProjectile newProjectile(){
		GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
		BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
		return(bullet);
	}
}
