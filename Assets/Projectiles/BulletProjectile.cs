﻿using UnityEngine;
using System.Collections;

public class BulletProjectile : Projectile, IShreddable {
	public float speed;
	public Vector3 vector;
	
	private Transform body;
	private SpriteRenderer bodySprite;
	
	public override void Start(){
		base.Start();
		affinity = owner.GetComponent<Entity>().affinity;
		body = transform.Find("Body");
		bodySprite = body.GetComponent<SpriteRenderer>();
		bodySprite.color = GetTeamColor();
		transform.parent = owner.transform.root.Find ("Bullets");
	}
	
	void Update () {
		transform.Translate (vector * Time.deltaTime * speed);
	}
	
	void OnTriggerEnter2D (Collider2D collision) {
		DetermineHit (collision, true);
	}
	
	protected override float DamageValue(){
		return(1);
	}
	
}
