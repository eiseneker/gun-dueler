using UnityEngine;
using System.Collections;

public class Chaingun : Weapon {
	private float defaultSpeed = 20;
	private float defaultFireDelay = 0.02f;
	private int defaultMaxBulletsInPlay = 100;
	
	private GameObject bulletPrefab;
	private float speed;
	private AudioClip soundClip;
	
	private float maxBulletCount = 100;
	private float currentBulletCount;
	
	public Chaingun(){
		maxBulletsInPlay = defaultMaxBulletsInPlay;
		fireDelay = defaultFireDelay;
		speed = defaultSpeed;
		bulletPrefab = Resources.Load ("bullet") as GameObject;
		soundClip = Resources.Load<AudioClip>("Vulcan");
		currentBulletCount = maxBulletCount;
	}
	
	protected override void Update(){
		base.Update ();
		if(currentBulletCount < maxBulletCount){
			currentBulletCount += Mathf.Pow(timeSinceLastFire/5, 2);
		}
		print (currentBulletCount);
	}
 
	public void Fire (bool exAttempt) {
		if(CanFire ()){
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
		if(currentBulletCount > 0){
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
			currentBulletCount--;
		}
	}
	
	public float currentBulletRatio(){
		return(currentBulletCount/maxBulletCount);
	}
}
