using UnityEngine;
using System.Collections;

public class Shotgun : Weapon {
	private float defaultSpeed = 8;
	private float speed;
	
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
	
	public void Fire (bool exAttempt) {
		if(CanFire ()){
			bool ex = exAttempt && player.SpendEx(25);
			speed = defaultSpeed;
			if(ex) speed *= 1.5f;
		
			foreach(Vector3 vector in bulletVectors){
				BulletProjectile bullet = newProjectile();
				bullet.speed = speed;
				bullet.weapon = this;
				bullet.vector = vector;
				bullet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
				OrientProjectile(bullet);
			}
			
			timeSinceLastFire = ex ? fireDelay / 2 : 0;
		}
	}
	
	private BulletProjectile newProjectile(){
		GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
		BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
		return(bullet);
	}
}
