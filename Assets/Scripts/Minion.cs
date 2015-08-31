using UnityEngine;
using System.Collections;

public class Minion : MonoBehaviour, IHarmable, IAttacker {
	public float speed;
	public GameObject bulletPrefab;
	public float bulletSpeed;
	public float fireDelay;
	public GameObject formation;
	
	private float timeSinceLastFire;
	private GameObject body;
	private float yVector = 1;
	
	void Start() {
		body = transform.Find ("Body").gameObject;
		body.GetComponent<ParticleSystem>().startColor = GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor;
	}
	
	void Update () {
		timeSinceLastFire += Time.deltaTime;
		Fire ();
	}
	
	public void ReceiveHit(float damage, GameObject attackerObject) {
		formation.GetComponent<Formation>().MinionDestroyed();
		IAttacker attacker = attackerObject.GetComponent(typeof(IAttacker)) as IAttacker;
		attacker.RegisterSuccessfulAttack(10);
		Destroy(transform.parent.gameObject);
	}
	
	private void Fire () {
		if(timeSinceLastFire >= fireDelay){
			GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
			BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
			bullet.speed = bulletSpeed;
			bullet.owner = gameObject;
			if(GetComponent<Entity>().reversePosition) {
				yVector = -1;
			}
			bullet.vector = Vector3.up * yVector;
			timeSinceLastFire = 0f;
		}
	}
	
	public void RegisterSuccessfulAttack(float value){
		//chuckle
	}
}
