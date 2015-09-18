using UnityEngine;
using System.Collections;

public class Bomb : Agent, IShreddable {

	private float maxHealth = 5;
	private float currentHealth;
	
	public Player owner;
	public Vector2 initialVelocity;
	private Collider2D hitbox;
	
	private float maxInvincibleTime = 1f;
	private float currentInvincibleTime;
	
	// Use this for initialization
	void Start () {
		hitbox = GetComponent<Collider2D>();
		currentHealth = maxHealth;
		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), owner.GetComponent<Collider2D>());
		GetComponent<Rigidbody2D>().AddForce (new Vector2(0, 50));
	}
	
	// Update is called once per frame
	void Update () {
		currentInvincibleTime += Time.deltaTime;
		if(currentInvincibleTime > maxInvincibleTime){
			Physics2D.IgnoreCollision(GetComponent<Collider2D>(), owner.GetComponent<Collider2D>(), false);
		}
	}
	
	void OnCollisionEnter2D(Collision2D collision) {
		IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
		if(harmedObject != null){
			Explode();
		}
		
	}
	
	void OnTriggerEnter2D(Collider2D collider) {
		IHarmable harmedObject = collider.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
		if(harmedObject != null){
			Explode();
		}
		
	}
	
	public override void ReceiveHit(float damage, GameObject attackerObject){
		currentHealth -= damage;
		if(currentHealth <= 0){
			Explode();
		}
	}
	
	private void Explode(){
		GameObject explosionObject = Instantiate (Resources.Load ("Explosion"), transform.position, Quaternion.identity) as GameObject;
		Explosion explosion = explosionObject.GetComponent<Explosion>();
		explosion.hazardous = true;
		Destroy (gameObject);
	}
	
	public void DestroyMe(){
		Destroy(gameObject);
	}
	
}
