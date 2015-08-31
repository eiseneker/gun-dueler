using UnityEngine;
using System.Collections;

public class GigaBeamProjectile : Projectile {
	public float speed;
	public Vector3 vector;
	
	private Transform body;
	private SpriteRenderer bodySprite;
	private float maxLifespan = 3f;
	private float lifespan = 0f;
	
	// Use this for initialization
	public override void Start () {	
		base.Start();
		GetComponent<SpriteRenderer>().color = GetTeamColor ();
		transform.parent.parent = owner.transform.root.Find ("Bullets");
		owner.GetComponent<Player>().LockInputs();
	}
	
	// Update is called once per frame
	void Update () {
		if(owner == null){
			Destroy (gameObject);
		}
	
		lifespan += Time.deltaTime;
	
		if(maxLifespan < lifespan){
			Destroy (gameObject);
			if(owner){
				owner.GetComponent<Player>().UnlockInputs();
			}
		}
	}
	
	void OnTriggerStay2D (Collider2D collision) {
		DetermineHit(collision, false);
	}
}
