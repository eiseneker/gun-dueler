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
	
	protected float timeSinceStart = 0;
	
	void Start() {
		body = transform.Find ("Body").gameObject;
		foreach(Transform child in body.transform){
			Exhaust exhaust = child.GetComponent<Exhaust>();
			if(exhaust){
				exhaust.SetColor(GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor);
			}
		}
	}
	
	public virtual void Update () {
		timeSinceLastFire += Time.deltaTime;
		timeSinceStart += Time.deltaTime;
		Fire ();
	}
	
	public void ReceiveHit(float damage, GameObject attackerObject) {
		formation.GetComponent<Formation>().MinionDestroyed();
		if(attackerObject){
			IAttacker attacker = attackerObject.GetComponent(typeof(IAttacker)) as IAttacker;
			if(attacker != null){
				attacker.RegisterSuccessfulAttack(10);
			}
		}
		Destroy(transform.parent.gameObject);
	}
	
	public void RegisterSuccessfulAttack(float value){
		//chuckle
	}
	
	protected void FaceObject(GameObject inputObject){
		Vector3 distance = inputObject.transform.position - transform.position;
		distance.Normalize();
		
		float zRotation = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, zRotation - 90);
	}
	
	private void Fire () {
		if(timeSinceLastFire >= fireDelay){
			GameObject bulletObject = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
			BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
			bullet.speed = bulletSpeed;
			bullet.owner = gameObject;
			bullet.vector = Vector3.up;
			timeSinceLastFire = 0f;
		}
	}
}