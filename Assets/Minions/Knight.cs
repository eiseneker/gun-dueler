using UnityEngine;
using System.Collections;

public class Knight : Minion {
	public override void Update(){
		base.Update();
		if(timeSinceStart >= 2f){
			GameObject enemyPlayer = GetComponent<Entity>().EnemyPlayer();
			if(enemyPlayer){
				FaceObject (enemyPlayer);
			}
		}
	}	
}
