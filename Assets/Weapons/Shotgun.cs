using UnityEngine;
using System.Collections;

public class Shotgun : MonoBehaviour {
	public Player player;
	
	private float timeSinceLastFire;
	private float fireDelay = 3f;
	private float bulletSpeed = 7;
	private GameObject bulletPrefab;
	private float yVector = 1;
	private Vector3[] bulletVectors = {
		new Vector3(0.1f, 1, 0),
		new Vector3(-0.1f, 1, 0),
		new Vector3(0.2f, .95f, 0),
		new Vector3(-0.2f, .95f, 0),
		new Vector3(0.3f, .9f, 0),
		new Vector3(-0.3f, .9f, 0),
	};
	
	
	void Start () {
		timeSinceLastFire = fireDelay;
		bulletPrefab = Resources.Load ("bullet") as GameObject;
	}
	
	void Update () {
		timeSinceLastFire += Time.deltaTime;
	}
	
	public void Fire () {
		if(timeSinceLastFire >= fireDelay){
			foreach(Vector3 vector in bulletVectors){
				GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
				Bullet bullet = bulletObject.GetComponent<Bullet>();
				bullet.speed = bulletSpeed;
				bullet.specialWeapon = this;
				bullet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
				if(player.reversePosition) {
					yVector = -1;
				}
				bullet.vector = vector * yVector;
			}
			
			timeSinceLastFire = 0f;
		}
	}
}
