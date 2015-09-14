using UnityEngine;
using System.Collections;

public class Vulcan : Weapon {
	private float defaultSpeed = 7;
	private float defaultFireDelay = 0.10f;
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
	}
 
	public void Fire (bool exAttempt) {
		if(CanFire ()){
			AudioSource.PlayClipAtPoint(soundClip, transform.position);
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
				CreateBullet (0);
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
		BulletProjectile bullet = newProjectile();
		
		bullet.speed = speed;
		bullet.weapon = this;
		bullet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
		float xMovement = player.GetComponent<Rigidbody2D>().velocity.magnitude;
		print(xMovement);
		bullet.vector = new Vector3(1, 1, 0);
		bullet.transform.position = new Vector3(transform.position.x + xOffset, transform.position.y, 0);
		RegisterBullet ();
		OrientProjectile(bullet);
	}
}
