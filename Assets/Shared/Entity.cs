using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	public GameObject affinity;
	public bool reversePosition;
	
	private GameObject enemyPlayer;
	
	void Start(){
		SetRotation ();
	}
	
	void SetRotation(){
		if(GetComponent<Entity>().reversePosition && transform.rotation.z == 0){
			gameObject.transform.eulerAngles = new Vector3(
				gameObject.transform.eulerAngles.x,
				gameObject.transform.eulerAngles.y,
				gameObject.transform.eulerAngles.z + 180);
		}
	}
	
	public GameObject EnemyPlayer(){
		GameObject player = enemyPlayer;
		if(player == null && GetComponent<Fleet>() == null){
			player = affinity.GetComponent<Entity>().EnemyPlayer() ;
		}
		if(player == null){
			player = FindEnemyPlayer();
		}
		enemyPlayer = player;
		return(player);
	}
	
	public void FaceObject(GameObject inputObject){
		Vector3 distance = inputObject.transform.position - transform.position;
		distance.Normalize();
		
		float zRotation = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, zRotation - 90);
	}
	
	private GameObject FindEnemyPlayer(){
		foreach(GameObject player in Player.players){
			if(player != affinity.GetComponent<Fleet>().player) {
				return(player);
			}
		}
		return(null);
	}
}
