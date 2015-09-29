using UnityEngine;
using System.Collections;

public class LoadMarker : MonoBehaviour {

	public Road road;
	
	private int hitCount = 0;

	void OnTriggerEnter2D(Collider2D collider){
		print (collider.gameObject);
		if(collider.GetComponent<GameMarker>()){
			hitCount++;
			if(hitCount == 1){
				road.LoadNewRoad();
			}else if(hitCount == 2){
				road.UnloadFirstRoad();			
			}		
		}
	}
}
