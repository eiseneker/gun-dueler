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
	
	private GameObject FindEnemyPlayer(){
		print (Player.players.Count);
		foreach(GameObject player in Player.players){
			if(player != affinity.GetComponent<Fleet>().player) {
				return(player);
			}
		}
		return(null);
	}
}
