using UnityEngine;
using System.Collections;

public class GigaLaser : MonoBehaviour {
	public float speed;
	public Vector3 vector;
	public Vulcan weapon;
	public GigaBeam specialWeapon;
	public GameObject owner;
	
	private GameObject affinity;
	private Transform body;
	private SpriteRenderer bodySprite;
	private GameObject target;
	private bool targetFound = false;
	private float xFactor = 1;
	private float maxLifespan = 3f;
	private float lifespan = 0f;

	// Use this for initialization
	void Start () {	
		if(specialWeapon){
			owner = specialWeapon.player.gameObject;
		}
//		
		affinity = owner.GetComponent<Entity>().affinity;
		GetComponent<SpriteRenderer>().color = affinity.GetComponent<Fleet>().teamColor;
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
	
//		transform.position = new Vector3(transform.position.x, startY - transform.localScale.y/2, transform.position.z);
	}
	
	void OnTriggerStay2D (Collider2D collision) {
		Entity hitEntity = collision.gameObject.GetComponent<Entity>();
		if(hitEntity){
			print (hitEntity.affinity + " vs. " + affinity);
			if(hitEntity.affinity != affinity){
				IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
				if(harmedObject != null){
					harmedObject.ReceiveHit(1);
				}
			}
//			if(collision.gameObject != owner && collision.gameObject.GetComponent<Bullet>() == null){
//				DestroyMe ();
//			}
		}
	}
}
