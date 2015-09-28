using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

	private float maxHealth = 100;
	private float currentHealth;
	
	void Start(){
		currentHealth = maxHealth;
	}
	
	void OnCollisionEnter2D(Collision2D collision){
		IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
		if(harmedObject != null){
			harmedObject.ReceiveHit(10, gameObject, gameObject);
			float damage = collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 15;
			currentHealth -= damage;
			print ("dinged for " +  damage);
			if(currentHealth <= 0){
				DestroyMe ();
			}
		}
	}
	
	void DestroyMe(){
		Destroy(gameObject);
	}
}
