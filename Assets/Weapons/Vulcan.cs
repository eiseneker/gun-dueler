using UnityEngine;
using System.Collections;

public class Vulcan : MonoBehaviour {
	public Player player;

	private int currentBulletsInPlay = 0;
	private float timeSinceLastFire;
	private float fireDelay = 0.1f;
	private float bulletSpeed = 8;
	private int maxBulletsInPlay = 8;
	private GameObject bulletPrefab;
 
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
				bullet.yVector = -1;
			}
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
