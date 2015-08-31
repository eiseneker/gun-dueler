using UnityEngine;
using System.Collections;

public class MagnetProjectile : Projectile {
	public float speed;
	public Vector3 vector;
	
	private Transform body;
	private SpriteRenderer bodySprite;
	private GameObject target;
	private bool targetFound = false;
	
	public override void Start(){
		base.Start();
		target = GetTarget ();
		if(target == null) DestroyMe ();
		body = transform.Find("Body");
		bodySprite = body.GetComponent<SpriteRenderer>();
		bodySprite.color = GetTeamColor ();
		transform.parent = owner.transform.root.Find ("Bullets");
	}
	
	void Update () {
		if(!targetFound){
			if(IsTargetNearby(target)){
				targetFound = true;
				float rotationFactor = -90;
				if(IsTargetToMyLeft()) rotationFactor *= -1;
				RotateMe(rotationFactor);
			}
		}
	
		transform.Translate (vector * Time.deltaTime * speed);
	}
	
	void OnTriggerEnter2D (Collider2D collision) {
		DetermineHit(collision, true);
	}
	
	private GameObject GetTarget(){
		foreach(GameObject player in Player.players){
			if(player != owner) {
				return(player);
			}
		}
		return(null);
	}
	
	private void RotateMe(float rotationFactor){
		transform.eulerAngles = new Vector3(
			transform.eulerAngles.x,
			transform.eulerAngles.y,
			transform.eulerAngles.z + rotationFactor);
	}
	
	private bool IsTargetToMyLeft(){
		return(
			(target.transform.position.x < gameObject.transform.position.x && transform.eulerAngles.z == 0) ||
			(target.transform.position.x > gameObject.transform.position.x && transform.eulerAngles.z.ToString () == "180")
		);
	}
	
	private bool IsTargetNearby(GameObject target){
		float distance = Mathf.Abs(target.transform.position.y - gameObject.transform.position.y);
		return(distance >= 0 && distance <= 0.2);
	}
}