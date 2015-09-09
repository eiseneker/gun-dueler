using UnityEngine;
using System.Collections;
using System;

public class Fleet : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject corePrefab;
	public Color teamColor;
	public float width;
	public float height;
	public int playerNumber;
	public GameObject player;
	public GameObject spawnTurretPrefab;

	private float maxHealth = 50;
	private float currentHealth;	
	private GameObject affinity;
	private bool reversePosition;
	private GameObject minions;
	private GameObject core;
	private float maxPlayerRespawnTime = 5;
	private float currentPlayerRespawnTime;
	private float[] structurePositions = { 
		2, 4, 6, 8, 10
	};
	
	private ArrayList corePositions = new ArrayList();

	void Start () {
		corePositions.Add(1);
	
		affinity = gameObject;
		reversePosition = GetComponent<Entity>().reversePosition;
		currentHealth = maxHealth;
		GetComponent<Entity>().affinity = gameObject;
		
		AddMinionsObject ();
		AddPlayer();
		AddStructures();
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
	
	void AddCore(Vector3 structurePosition){
		Transform border = transform.Find ("Ship Border");
		Vector3 position = structurePosition;
		core = Instantiate (corePrefab, Vector3.zero, Quaternion.identity) as GameObject;
		core.transform.parent = border;
		core.transform.localPosition = position;
		core.GetComponent<Entity>().affinity = gameObject;
		core.GetComponent<Entity>().reversePosition = reversePosition;
	}
	
	void AddSpawnTurret(Vector3 structurePosition){
		Transform border = transform.Find ("Ship Border");
		Vector3 position = structurePosition;
		GameObject spawnTurret  = Instantiate(spawnTurretPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		spawnTurret.transform.parent = border;
		spawnTurret.transform.localPosition = position;
		spawnTurret.GetComponent<Entity>().affinity = affinity;
		spawnTurret.GetComponent<Entity>().reversePosition = reversePosition;
	}
	
	void AddStructures(){
		int index = 1;
		foreach(float structurePosition in structurePositions){
			Vector3 position = new Vector3(structurePosition, 0, 0);
			if(corePositions.Contains (index)){
				AddCore (position);
			}else{
				AddSpawnTurret(position);
			}
			index++;
		}
	}
	
	public void ReceiveDamage(float damage){
		currentHealth -= damage;
	}
	
	public float CurrentHealthRatio(){
		return(currentHealth / maxHealth);
	}
}
