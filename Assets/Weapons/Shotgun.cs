using UnityEngine;
using System.Collections;

public class Shotgun : Weapon {
	private float defaultSpeed = 9;
	private float speed;
	
	private GameObject bulletPrefab;
	
	private float[] angles = {
		10, 25
	};
	
	public Shotgun() {
		fireDelay = 2f;
		bulletPrefab = Resources.Load ("bullet") as GameObject;
	}
	
	public void Fire (bool exAttempt) {
		if(CanFire ()){
			bool ex = exAttempt && player.SpendEx(25);
			speed = defaultSpeed;
			if(ex) speed *= 1.5f;
		
			foreach(float angle in angles){
				CreateBullet (angle, Vector3.down);
				CreateBullet (-angle, Vector3.down);
				if(ex){
					CreateBullet (angle, Vector3.up);
					CreateBullet (-angle, Vector3.up);
				}
			}
			
			timeSinceLastFire = ex ? fireDelay / 2 : 0;
		}
	}
	
	
	private void CreateBullet(float angle, Vector3 vector){
		BulletProjectile bullet = newProjectile();
		bullet.speed = speed;
		bullet.weapon = this;
		bullet.vector = vector;
		bullet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
		OrientProjectile(bullet);
		bullet.RotateMe (angle);
	}
	
	private BulletProjectile newProjectile(){
		GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
		BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
		return(bullet);
	}
}
