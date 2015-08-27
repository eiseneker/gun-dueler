using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {
	public float speed;
	public Vector3 vector;
	public Vulcan weapon;
	public MagnetMissile specialWeapon;
	public GameObject owner;
	
	private GameObject affinity;
	private Transform body;
	private SpriteRenderer bodySprite;
	private GameObject target;
	private bool targetFound = false;
	private float xFactor = 1;
	
	void Start(){
		if(specialWeapon){
			owner = specialWeapon.player.gameObject;
		}
		foreach(GameObject player in Player.players){
			if(player != owner) {
				target = player;
			}
		}
		affinity = owner.GetComponent<Entity>().affinity;
		body = transform.Find("Body");
		bodySprite = body.GetComponent<SpriteRenderer>();
		bodySprite.color = affinity.GetComponent<Fleet>().teamColor;
		transform.parent = owner.transform.root.Find ("Bullets");
	}
	
	void Update () {
	
		if(!targetFound){
			float distance = Mathf.Abs(target.transform.position.y - gameObject.transform.position.y);
			if(distance >= 0 && distance <= 0.2){
				targetFound = true;
				float rotationFactor = -90;
				
				if(target.transform.position.x < gameObject.transform.position.x && transform.eulerAngles.z == 0){
					rotationFactor *= -1;
				}else if(target.transform.position.x > gameObject.transform.position.x && transform.eulerAngles.z.ToString () == "180"){
					rotationFactor *= -1;
				}
				transform.eulerAngles = new Vector3(
				transform.eulerAngles.x,
				transform.eulerAngles.y,
				transform.eulerAngles.z + rotationFactor);
			}
		}
	
	
		if(targetFound){
		}
//		if(targetFound){
//			transform.Translate (Vector3.right * xFactor * Time.deltaTime * speed);
//		}else{
			transform.Translate (vector * Time.deltaTime * speed);
//		}
	}
	
	void OnTriggerEnter2D (Collider2D collision) {
		Entity hitEntity = collision.gameObject.GetComponent<Entity>();
		if(hitEntity){
			if(hitEntity.affinity != affinity){
				IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
				if(harmedObject != null){
					harmedObject.ReceiveHit(1);
				}
			}
			if(collision.gameObject != owner && collision.gameObject.GetComponent<Bullet>() == null){
				DestroyMe ();
			}
		}
	}
	
	public void DestroyMe(){
		if(weapon){
			weapon.UnregisterBullet();
		}
		Destroy(gameObject);
	}
}
