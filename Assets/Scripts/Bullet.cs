using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float speed = 1f;
	public float yVector = 1;
	public GameObject owner;
	
	private GameObject affinity;
	private Transform body;
	private SpriteRenderer bodySprite;
	
	void Start(){
		affinity = owner.GetComponent<Entity>().affinity;
		body = transform.Find("Body");
		bodySprite = body.GetComponent<SpriteRenderer>();
		bodySprite.color = affinity.GetComponent<Fleet>().teamColor;
		transform.parent = owner.transform.root.Find ("Bullets");
	}
	
	void Update () {
		transform.Translate (Vector3.up * Time.deltaTime * speed * yVector);
	}
	
	void OnTriggerEnter2D (Collider2D collision) {
		Entity hitEntity = collision.gameObject.GetComponent<Entity>();
		if(hitEntity){
			if(hitEntity.affinity != affinity){
				IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
				if(harmedObject != null){
					harmedObject.ReceiveHit();
				}
			}
			if(collision.gameObject != owner && collision.gameObject.GetComponent<Bullet>() == null){
				DestroyMe ();
			}
		}
	}
	
	public void DestroyMe(){
		if(owner){
			Player player = owner.GetComponent<Player>();
			if(player){
				player.UnregisterBullet();
			}
		}
		Destroy(gameObject);
	}
}
