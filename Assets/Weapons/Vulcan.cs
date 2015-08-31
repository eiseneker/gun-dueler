using UnityEngine;
using System.Collections;

public class Vulcan : Weapon {
	private float bulletSpeed = 8;
	private GameObject bulletPrefab;
	
	public Vulcan(){
		maxBulletsInPlay = 1;
		fireDelay = 0.3f;
		bulletPrefab = Resources.Load ("bullet") as GameObject;
	}
 
	public void Fire () {
		if(CanFire ()){
			BulletProjectile bullet = newProjectile();
			bullet.speed = bulletSpeed;
			bullet.weapon = this;
			bullet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
			bullet.vector = Vector3.up;
			RegisterBullet ();
			RotateProjectile(bullet);
			timeSinceLastFire = 0f;
		}
	}
	
	private BulletProjectile newProjectile(){
		GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
		BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
		return(bullet);
	}
}
