using UnityEngine;
using System.Collections;

public class GameMarkerCamera : MonoBehaviour {

	private Vector3  currentVelocity = new Vector3(0, 0, 0);
	private GameObject gameMarker;
	
	void Start(){
		gameMarker = GameObject.Find("Game Root/GameMarker");
	}

	void LateUpdate () {
			Vector3 destination = new Vector3(gameMarker.transform.position.x, transform.position.y, transform.position.z);
//			transform.position = Vector3.SmoothDamp(transform.position, destination, ref currentVelocity, 0.1f);
			transform.position = destination;
	}
	
}
