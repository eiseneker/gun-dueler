using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	public GameObject player;
	
	public int playerNumber;
	private Vector3  currentVelocity = new Vector3(0, 0, 0);

	void LateUpdate () {
		if(player){
			Vector3 destination = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref currentVelocity, 0.1f);
		}else{
			player = GetPlayer();
		}
	}
	
	private GameObject GetPlayer(){
		return(GameObject.Find ("Game Root/Players/Player " + playerNumber + " Fleet/Player(Clone)"));
	}
	
}
