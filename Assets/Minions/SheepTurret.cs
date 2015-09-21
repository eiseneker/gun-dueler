using UnityEngine;
using System.Collections;

public class SheepTurret : MonoBehaviour {
	public GameObject bulletPrefab;
	public float bulletSpeed;
	public GameObject owner;
	
	public void CreateBullet(float xVelocity){
		GameObject bulletObject = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
		BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
		bullet.speed = bulletSpeed;
		bullet.owner = owner;
		bullet.yVector = 1;
		bullet.xVector = xVelocity;
		print (bullet.owner);
	}
}
