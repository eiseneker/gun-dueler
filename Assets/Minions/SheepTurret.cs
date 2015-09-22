using UnityEngine;
using System.Collections;

public class SheepTurret : MonoBehaviour {
	public GameObject bulletPrefab;
	public float bulletSpeed;
	public GameObject owner;
	
	public void Start(){
		
	}
	
	public void Update(){
		Vector3 forward = transform.TransformDirection(Vector3.up) * 10;
		Debug.DrawRay(transform.position, forward, Color.green);
	}
	
	public void CreateBullet(float xVelocity){
		GameObject bulletObject = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
		BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
		bullet.speed = bulletSpeed;
		bullet.owner = owner;
		bullet.GetComponent<Rigidbody2D>().velocity = owner.GetComponent<Rigidbody2D>().velocity;
		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), owner.GetComponent<Collider2D>());
		float bulletMagnitude = .25f;
		bullet.yVector = bulletMagnitude;
		bullet.GetComponent<Rigidbody2D>().velocity = owner.GetComponent<Rigidbody2D>().velocity;
	}
}
