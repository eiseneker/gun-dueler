using UnityEngine;
using System.Collections;

public class MagnetProjectile : Projectile, IShreddable {
	public float speed;
	public Vector3 vector;
	public bool defaultOrientation = true;
	
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
				float rotationFactor;
				targetFound = true;
				rotationFactor = -90;
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
	
	private bool IsTargetToMyLeft(){
		bool isLeft;
		
		if(defaultOrientation){
			isLeft = (target.transform.position.x < gameObject.transform.position.x && OrientationHelper.FacingUp (transform)) ||
				(target.transform.position.x > gameObject.transform.position.x && OrientationHelper.FacingDown (transform));
		}else{
			isLeft = (target.transform.position.y > gameObject.transform.position.y && OrientationHelper.FacingRight (transform)) ||
				(target.transform.position.y < gameObject.transform.position.y && OrientationHelper.FacingLeft (transform));
		}
		
		return(isLeft);
	}
	
	private bool IsTargetNearby(GameObject target){
		float distance;
		if(defaultOrientation){
			distance = Mathf.Abs(target.transform.position.y - gameObject.transform.position.y);
		}else{
			distance = Mathf.Abs(target.transform.position.x - gameObject.transform.position.x);
		}
		return(distance >= 0 && distance <= 0.2);	
		
	}
}