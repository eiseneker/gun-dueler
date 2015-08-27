using UnityEngine;
using System.Collections;

public class Vulcan : MonoBehaviour {
	public Player player;

	private int currentBulletsInPlay = 0;
	private float timeSinceLastFire;
	private float fireDelay = 0.3f;
	private float bulletSpeed = 8;
	private int maxBulletsInPlay = 8;
	private GameObject bulletPrefab;
	private float yVector = 1;
 
	void Start () {
		bulletPrefab = Resources.Load ("bullet") as GameObject;
	}
	
	void Update () {
		timeSinceLastFire += Time.deltaTime;
	}
	
	public void Fire () {
		if(!AtMaxBullets() && timeSinceLastFire >= fireDelay){
			GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
			Bullet bullet = bulletObject.GetComponent<Bullet>();
			bullet.speed = bulletSpeed;
			bullet.weapon = this;
			bullet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
			if(player.reversePosition) {
				yVector = -1;
			}
			bullet.vector = Vector3.up * yVector;
			RegisterBullet ();
			timeSinceLastFire = 0f;
		}
	}
	
	public void RegisterBullet(){
		currentBulletsInPlay++;
	}
	
	public void UnregisterBullet(){
		currentBulletsInPlay--;
	}
	
	private bool AtMaxBullets(){
		return(currentBulletsInPlay >= maxBulletsInPlay);
	}
}
