using UnityEngine;
using System.Collections;

public class Sheep : Minion {

	private float currentMoveTime;
	private float maxMoveTime = 3;
	private float firesPerSecond = 2f;
	private bool skipDestroyOnMerge = false;

	public override void Update () {
		base.Update ();
		currentMoveTime += Time.deltaTime;
		
		if(currentMoveTime < maxMoveTime){
			transform.Translate(Vector3.up * Time.deltaTime);
		}
		
		float probability = firesPerSecond * Time.deltaTime;
		
		if(Random.value < probability){
			Fire ();
		}
	}
	
	void OnCollisionEnter2D (Collision2D collision){
		if(collision.gameObject.GetComponent<Entity>()){
			if(collision.gameObject.GetComponent<Entity>().affinity == GetComponent<Entity>().affinity){
				Sheep sheep = collision.gameObject.GetComponent<Sheep>();
				if(sheep){
					sheep.Upgrade();
					if(!skipDestroyOnMerge){
						Destroy (gameObject);
					}
				}
			}
			skipDestroyOnMerge = false;
		}
	}
	
	protected void Upgrade(){
		skipDestroyOnMerge = true;
		damageBehavior.IncreaseMaxHealth(2);
		damageBehavior.HealToFull();
		firesPerSecond += 1;
		transform.localScale += new Vector3(0.1f, 0.1f, 0);
		rigidbody2D.mass += 100;
	}
}
