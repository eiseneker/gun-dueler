using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	public GameObject player;
	
	public int playerNumber;

	void Update () {
		if(player){
			transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
		}else{
			player = GetPlayer();
		}
	}
	
	private GameObject GetPlayer(){
		return(GameObject.Find ("Game Root/Players/Player " + playerNumber + " Fleet/Player(Clone)"));
	}
}
