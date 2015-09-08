
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
	private SpriteRenderer bodySprite;
	private float maxHealth = 5;
	private float currentHealth;
	private float maxDamageAnimationTimer = 0.1f;
	private float currentDamageAnimationTimer;
	private Color normalColor;
	private Color whiteColor = new Color(1, 0, 0);
	
	protected float timeSinceStart = 0;
	
	
	void Start() {
		currentDamageAnimationTimer = maxDamageAnimationTimer;
		currentHealth = maxHealth;
		body = transform.Find ("Body").gameObject;
		foreach(Transform child in body.transform){
			Exhaust exhaust = child.GetComponent<Exhaust>();
			if(exhaust){
				exhaust.SetColor(GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor);
			}
		}
		bodySprite = body.GetComponent<SpriteRenderer>();
		normalColor = bodySprite.color;
	}
	
	public virtual void Update () {
		timeSinceLastFire += Time.deltaTime;
		timeSinceStart += Time.deltaTime;
		currentDamageAnimationTimer += Time.deltaTime;
		if(currentDamageAnimationTimer < maxDamageAnimationTimer){
			bodySprite.color = whiteColor;
		}else{
			bodySprite.color = normalColor;
		}
		Fire ();
	}
	
	public void ReceiveHit(float damage, GameObject attackerObject) {
		IAttacker attacker;
		formation.GetComponent<Formation>().MinionDestroyed();
		if(attackerObject){
			attacker = attackerObject.GetComponent(typeof(IAttacker)) as IAttacker;
			if(attacker != null){
				attacker.RegisterSuccessfulAttack(0);
			}
		}
		if(attackerObject.GetComponent<Minion>()){
			damage /= 2;
		}
		currentHealth -= damage;
		currentDamageAnimationTimer = 0;
		if(currentHealth <= 0){
			attacker = attackerObject.GetComponent(typeof(IAttacker)) as IAttacker;
			if(attacker != null){
				attacker.RegisterSuccessfulDestroy(5);
			}
			Destroy(transform.parent.gameObject);
		}
	}
	
	public void RegisterSuccessfulAttack(float value){
		//chuckle
	}
	
	public void RegisterSuccessfulDestroy(float value){
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