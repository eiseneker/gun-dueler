using UnityEngine;
using System.Collections;

public class Bull : Minion {
	GameObject enemyPlayer;
	private float speed;
	
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
	
	private void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject == enemyPlayer){
			IHarmable harmedObject = collider.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
			if(harmedObject != null){
				harmedObject.ReceiveHit(1, gameObject);
			}
			DestroyMe();
		}
	}
}
