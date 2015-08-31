using UnityEngine;
using System.Collections;

public class Shotgun : Weapon {
	private float bulletSpeed = 7;
	private GameObject bulletPrefab;
	private Vector3[] bulletVectors = {
		new Vector3(0.1f, 1, 0),
		new Vector3(-0.1f, 1, 0),
		new Vector3(0.2f, .95f, 0),
		new Vector3(-0.2f, .95f, 0),
		new Vector3(0.3f, .9f, 0),
		new Vector3(-0.3f, .9f, 0),
	};
	
	public Shotgun() {
		fireDelay = 3f;
		bulletPrefab = Resources.Load ("bullet") as GameObject;
	}
	
	public void Fire () {
		if(CanFire ()){
			foreach(Vector3 vector in bulletVectors){
				BulletProjectile bullet = newProjectile();
				bullet.speed = bulletSpeed;
				bullet.weapon = this;
				bullet.vector = vector;
				bullet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
				RotateProjectile(bullet);
			}
			
			timeSinceLastFire = 0f;
		}
	}
	
	private BulletProjectile newProjectile(){
		GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
		BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
		return(bullet);
	}
}
