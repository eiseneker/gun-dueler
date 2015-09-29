using UnityEngine;
using System.Collections;

public class BoundaryController : MonoBehaviour {

	private GameObject gameMarker;

	void Start(){
		gameMarker = GameObject.Find("Game Root/GameMarker");
	}
	
	void Update () {
		transform.position = gameMarker.transform.position;
	}
}
