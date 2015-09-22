using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletTurret : Structure {
	private bool disabled = false;
	private DamageBehavior damageBehavior;
	private SpriteRenderer bodySprite;
	private float currentFireCooldown;
	private GameObject player;
	private Fleet fleet;
	public float maxFireCooldown;
	public float maxFireDelay;
	private float currentFireDelay;
	public float maxFireAmount;
	private float currentFireAmount;
	private GameObject bullets;
	private GameObject bulletPrefab;
	
	void Start(){
		damageBehavior = GetComponent<DamageBehavior>();
		bodySprite = transform.Find ("Body").GetComponent<SpriteRenderer>();
		bullets = GameObject.Find ("Game Root/Bullets");
		fleet = GetComponent<Entity>().affinity.GetComponent<Fleet>();
		bulletPrefab = Resources.Load ("Bullet") as GameObject;
	}
	
	void Update () {
		if(GameController.gameStarted && !disabled){
			if(currentFireAmount < maxFireAmount){
				currentFireDelay += Time.deltaTime;
				if(currentFireDelay >= maxFireDelay){
					Fire();
					currentFireAmount++;
					currentFireDelay = 0;
					currentFireCooldown = 0;
				}
			}else{
				currentFireCooldown += Time.deltaTime;
				if(currentFireCooldown >= maxFireCooldown){
					currentFireAmount = 0;
				}
			}
		}else{
			currentFireCooldown = 0;
		}
		bodySprite.color = NormalColor();
	}
	
	void Fire(){
		GameObject bulletObject = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
		BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
		bullet.owner = gameObject;
		bullet.GetComponent<Rigidbody2D>().velocity = truck.GetComponent<Rigidbody2D>().velocity;
		float bulletMagnitude = .5f;
		bullet.yVector = bulletMagnitude;
		Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), truck.GetComponent<Collider2D>());
		bullet.transform.parent = bullets.transform;
	}
	
	public override void ReceiveHit(float damage, GameObject attackerObject, GameObject attack) {
		if(!disabled){
			IAttacker attacker = ResolveAttacker(attackerObject);
			if(attacker != null) attacker.RegisterSuccessfulAttack(0);
			damageBehavior.ReceiveDamage(damage);
			
			if(damageBehavior.CurrentHealthRatio() <= 0){
				disabled = true;
				if(attacker != null) attacker.RegisterSuccessfulDestroy(5);
				DestroyMe ();
			}
		}
	}
	
	private void DestroyMe(){
		fleet.ReceiveDamage(5);
		GameObject explosion = Instantiate ( Resources.Load ("Explosion"), transform.position, Quaternion.identity) as GameObject;
		explosion.transform.localScale -= new Vector3(0.5f, 0.5f, 0);
	}
	
	private Color NormalColor(){
		float healthRatio = damageBehavior.CurrentHealthRatio();
		Color color;
		if(healthRatio > 0){
			color = new Color(1, healthRatio, healthRatio);
		}else{
			color = new Color(0.3f, 0.3f, 0.3f);
		}
		return(color);
	}
}
