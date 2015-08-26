using UnityEngine;
using System.Collections;

public class Minion : MonoBehaviour, IHarmable {
	public float speed;
	public GameObject bulletPrefab;
	public float bulletSpeed;
	public float fireDelay;
	public GameObject formation;
	
	private float timeSinceLastFire;
	private GameObject body;
	
	void Start() {
		body = transform.Find ("Body").gameObject;
		body.GetComponent<ParticleSystem>().startColor = GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor;
	}
	
	void Update () {
		timeSinceLastFire += Time.deltaTime;
		Fire ();
	}
	
	public void ReceiveHit() {
		formation.GetComponent<Formation>().MinionDestroyed();
		Destroy(transform.parent.gameObject);
	}
	
	private void Fire () {
		if(timeSinceLastFire >= fireDelay){
			GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
			Bullet bullet = bulletObject.GetComponent<Bullet>();
			bullet.speed = bulletSpeed;
			bullet.owner = gameObject;
			if(GetComponent<Entity>().reversePosition) {
				bullet.yVector = -1;
			}
			timeSinceLastFire = 0f;
		}
	}
}
