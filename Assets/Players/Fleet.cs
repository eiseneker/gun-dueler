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
	private int structureCount = 12;
	private float structureSpacing = 2;
	private ArrayList structurePositions = new ArrayList();
	private int coreInterval = 5;
	private GameObject shipBorder;
	private ArrayList structures = new ArrayList();
	private float lastStructureX;

	void Start () {
		for(int i = 0 - structureCount/2; i < structureCount/2; i++){
			structurePositions.Add (i * structureSpacing);
		}
	
		affinity = gameObject;
		reversePosition = GetComponent<Entity>().reversePosition;
		currentHealth = maxHealth;
		shipBorder = transform.Find ("Ship Border").gameObject;
		
		GetComponent<Entity>().affinity = gameObject;
		
		AddMinionsObject ();
		AddPlayer();
		AddStructures();
		
		lastStructureX = shipBorder.transform.position.x;
	}
	
	void Update() {
		if(player == null){
			if(currentPlayerRespawnTime >= maxPlayerRespawnTime){
				AddPlayer ();
			}else{
				currentPlayerRespawnTime += Time.deltaTime;
			}
		}
		GameObject lastStructure = structures[structures.Count - 1] as GameObject;
		GameObject firstStructure = structures[0] as GameObject;
		shipBorder.transform.Translate(Vector3.right * Time.deltaTime * .5f);
		if(lastStructure.transform.position.x > 6 && !reversePosition || lastStructure.transform.position.x < -6 && reversePosition){
			structures.Remove (lastStructure);
			structures.Insert (0, lastStructure);
			Vector3 firstPosition = firstStructure.transform.localPosition;
			lastStructure.transform.localPosition = new Vector3(firstPosition.x - structureSpacing, firstPosition.y, 0);
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
		core = Instantiate (corePrefab, Vector3.zero, Quaternion.identity) as GameObject;
		core.transform.parent = shipBorder.transform;
		core.transform.localPosition = structurePosition;
		core.GetComponent<Entity>().affinity = gameObject;
		core.GetComponent<Entity>().reversePosition = reversePosition;
		structures.Add (core);
	}
	
	void AddSpawnTurret(Vector3 structurePosition){
		GameObject spawnTurret  = Instantiate(spawnTurretPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		spawnTurret.transform.parent = shipBorder.transform;
		spawnTurret.transform.localPosition = structurePosition;
		spawnTurret.GetComponent<Entity>().affinity = affinity;
		spawnTurret.GetComponent<Entity>().reversePosition = reversePosition;
		structures.Add (spawnTurret);
	}
	
	void AddStructures(){
		int index = 1;
		foreach(float structurePosition in structurePositions){
			Vector3 position = new Vector3(structurePosition, 0, 0);
			if(index % coreInterval == 0){
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
