using UnityEngine;
using System.Collections;

public class LoadMarker : MonoBehaviour {

	public Road road;
	
	private int hitCount = 0;

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.GetComponent<Structure>()){
			hitCount++;
			if(hitCount == 1){
				road.LoadNewRoad();
			}else if(hitCount == Structure.structureCount){
				road.UnloadFirstRoad();			
			}		
		}
	}
}
