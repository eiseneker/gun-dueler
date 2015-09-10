using UnityEngine;
using System.Collections;

public class Bull : Minion {
	GameObject enemyPlayer;
	
	public override void Start(){
		base.Start();
		speed = 2;
		enemyPlayer = GetComponent<Entity>().EnemyPlayer();
	}

	public override void Update(){
		base.Update();
		if(timeSinceStart >= 2f){
			speed = 5;
			if(enemyPlayer){
				GetComponent<Entity>().FaceObject (enemyPlayer);
			}
		}
		transform.Translate(Vector3.up * Time.deltaTime * speed);
	}
	
	private void OnCollisionEnter2D(Collision2D collision){
		if(collision.gameObject.GetComponent<Entity>().affinity != GetComponent<Entity>().affinity){
			IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
			if(harmedObject != null){
				harmedObject.ReceiveHit(1, gameObject);
			}
			DestroyMe();
		}
	}
}
