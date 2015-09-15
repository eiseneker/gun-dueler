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
			bool ex = exAttempt;
			speed = defaultSpeed;
			if(ex) speed *= 1.5f;
		
			foreach(float angle in angles){
				CreateBullet (angle, -1);
				CreateBullet (-angle, -1);
				if(ex){
					CreateBullet (angle, 1);
					CreateBullet (-angle, 1);
				}
			}
			
			timeSinceLastFire = ex ? fireDelay / 2 : 0;
		}
	}
	
	
	private void CreateBullet(float angle, float yVector){
		BulletProjectile bullet = newProjectile();
		bullet.speed = speed;
		bullet.weapon = this;
		bullet.yVector = yVector;
		bullet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
		float xMovement = player.GetComponent<Rigidbody2D>().velocity.magnitude;
		bullet.xVector = Mathf.Round (xMovement);
		OrientProjectile(bullet);
		bullet.RotateMe (angle);
	}
	
	private BulletProjectile newProjectile(){
		GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
		BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
		return(bullet);
	}
}
