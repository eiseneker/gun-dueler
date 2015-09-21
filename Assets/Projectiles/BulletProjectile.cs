using UnityEngine;
using System.Collections;

public class BulletProjectile : Projectile, IShreddable {
	public float speed;
	public float xVector;
	public float yVector;
	
	private Transform body;
	private SpriteRenderer bodySprite;
	
	public override void Start(){
		base.Start();
		GetComponent<Entity>().affinity = owner.GetComponent<Entity>().affinity;
		body = transform.Find("Body");
		bodySprite = body.GetComponent<SpriteRenderer>();
		bodySprite.color = GetTeamColor();
		transform.parent = owner.transform.root.Find ("Bullets");
		GetComponent<Rigidbody2D>().velocity = owner.GetComponent<Rigidbody2D>().velocity;
		GetComponent<Rigidbody2D>().AddRelativeForce (new Vector2(0, yVector));
	}
	
	public override void Update () {
		base.Update ();
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		DetermineHit (collider, true);
	}
	
	void OnCollisionEnter2D (Collision2D collision) {
		DetermineHit (collision, true);
	}
	
	protected override float DamageValue(){
		return(0.5f);
	}
	
}
