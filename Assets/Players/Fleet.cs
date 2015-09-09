using UnityEngine;
using System.Collections;

public class Fleet : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject corePrefab;
	public Color teamColor;
	public float width;
	public float height;
	public int playerNumber;
	public GameObject player;
	public GameObject spawnTurretPrefab;
	
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
		AddSpawnTurrets();
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
		core = Instantiate (corePrefab, transform.Find ("Ship Border").Find ("Core Position").position, Quaternion.identity) as GameObject;
		core.transform.parent = transform;
		core.GetComponent<Entity>().affinity = gameObject;
	}
	
	void AddSpawnTurrets(){
		foreach(Transform child in transform.Find ("Ship Border").Find ("Spawn Turret Positions")){
			GameObject spawnTurret  = Instantiate(spawnTurretPrefab, child.position, Quaternion.identity) as GameObject;
			spawnTurret.GetComponent<Entity>().affinity = affinity;
			spawnTurret.GetComponent<Entity>().reversePosition = reversePosition;
			spawnTurret.GetComponent<SpawnTurret>().level = child.GetComponent<Position>().level;
		}
		
	}
}
