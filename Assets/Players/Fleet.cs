using UnityEngine;
using System.Collections;

public class Fleet : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject corePrefab;
	public GameObject[] minionFormations;
	public Color teamColor;
	public float width;
	public float height;
	public int playerNumber;
	public GameObject player;
	
	private GameObject affinity;
	private bool reversePosition;
	private GameObject minions;
	
	private GameObject core;
	
	private float maxPlayerRespawnTime = 5;
	private float currentPlayerRespawnTime;

	void Start () {
		affinity = gameObject;
		reversePosition = GetComponent<Entity>().reversePosition;
		GetComponent<Entity>().affinity = gameObject;
		
		AddMinionsObject ();
		AddPlayer();
		AddCore();
		AddMinionFormation (0);
	}
	
	void Update() {
		if(player == null){
			if(currentPlayerRespawnTime >= maxPlayerRespawnTime){
				AddPlayer ();
			}else{
				currentPlayerRespawnTime += Time.deltaTime;
			}
		}
	}
	
	void OnDrawGizmos () {
		Gizmos.DrawWireCube(
			transform.position,
			new Vector3(width, height)
			);
	}
	
	void AddMinionsObject(){
		minions = new GameObject("Minions");
		minions.transform.position = transform.position;
		minions.transform.parent = transform;
	}
	
	void AddPlayer(){
		player = Instantiate (playerPrefab, transform.Find ("Player Position").position, Quaternion.identity) as GameObject;
		player.transform.parent = transform;
		player.GetComponent<Entity>().affinity = gameObject;
		player.GetComponent<Entity>().reversePosition = GetComponent<Entity>().reversePosition;
		player.GetComponent<Player>().SetPlayerNumber(playerNumber);
		currentPlayerRespawnTime = 0;
		
	}
	
	void AddCore(){
		core = Instantiate (corePrefab, transform.Find ("Core Position").position, Quaternion.identity) as GameObject;
		core.transform.parent = transform;
		core.GetComponent<Entity>().affinity = gameObject;
	}
	
	public GameObject AddMinionFormation(int index){
		GameObject minionFormation  = Instantiate(minionFormations[index], transform.Find ("Formation Position").position, Quaternion.identity) as GameObject;
		minionFormation.transform.parent = minions.transform;
		minionFormation.GetComponent<Entity>().affinity = affinity;
		minionFormation.GetComponent<Entity>().reversePosition = reversePosition;
		return(minionFormation);
	}
}
