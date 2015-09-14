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
	public GameObject[] structures;
	public float lastExValue;

	private float maxHealth = 50;
	private float currentHealth;	
	private bool reversePosition;
	private GameObject minions;
	private GameObject core;
	private float maxPlayerRespawnTime = 5;
	private float currentPlayerRespawnTime;
	private float structureSpacing = 2;
	private ArrayList structurePositions = new ArrayList();
	private ArrayList structureList = new ArrayList();
	private ArrayList structuresInPlay = new ArrayList();
	private GameObject shipBorder;
	private float rotationFactor = -0.5f;

	void Start () {
		structureList.AddRange (structures);
	
		for(int i = 0 - structures.Length/2; i < structures.Length/2; i++){
			structurePositions.Add (i * structureSpacing);
		}
	
		reversePosition = GetComponent<Entity>().reversePosition;
		currentHealth = maxHealth;
		shipBorder = transform.Find ("Ship Border").gameObject;
		
		GetComponent<Entity>().affinity = gameObject;
		
		AddMinionsObject ();
		AddPlayer();
//		AddStructures();
	}
	
	void Update() {
		if(player == null){
			if(currentPlayerRespawnTime < maxPlayerRespawnTime){
				currentPlayerRespawnTime += Time.deltaTime;
			}
		}
		GameObject lastStructure = structuresInPlay[structures.Length - 1] as GameObject;
		GameObject firstStructure = structuresInPlay[0] as GameObject;
		
		rotationFactor = Mathf.Clamp (rotationFactor - Input.GetAxis ("Player"+playerNumber+"_MoveFleetLeft")/10 + Input.GetAxis ("Player"+playerNumber+"_MoveFleetRight")/10, -5, 5);
		
//		shipBorder.transform.Translate(Vector3.right * Time.deltaTime * rotationFactor);
		
//		if(rotationFactor > 0){
//			if(lastStructure.transform.position.x > 6 && !reversePosition || lastStructure.transform.position.x < -6 && reversePosition){
//				structuresInPlay.Remove (lastStructure);
//				structuresInPlay.Insert (0, lastStructure);
//				Vector3 firstPosition = firstStructure.transform.localPosition;
//				lastStructure.transform.localPosition = new Vector3(firstPosition.x - structureSpacing, firstPosition.y, 0);
//			}
//		}else{
//			if(firstStructure.transform.position.x < -6 && !reversePosition || firstStructure.transform.position.x > 6 && reversePosition){
//				structuresInPlay.Remove (firstStructure);
//				structuresInPlay.Add (firstStructure);
//				Vector3 lastPosition = lastStructure.transform.localPosition;
//				firstStructure.transform.localPosition = new Vector3(lastPosition.x + structureSpacing, lastPosition.y, 0);
//			}
//		}
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
	
	public GameObject AddPlayer(){
		player = Instantiate (playerPrefab, transform.Find ("Player Position").position, Quaternion.identity) as GameObject;
		player.transform.parent = transform;
		player.GetComponent<Entity>().affinity = gameObject;
		player.GetComponent<Entity>().reversePosition = GetComponent<Entity>().reversePosition;
		player.GetComponent<Player>().SetPlayerNumber(playerNumber);
		currentPlayerRespawnTime = 0;
		ResetPlayerRespawn();
		return(player);
	}
	
	void AddStructure(Vector3 structurePosition, GameObject structurePrefab){
		GameObject structure = Instantiate (structurePrefab, Vector3.zero, Quaternion.identity) as GameObject;
		structure.transform.parent = shipBorder.transform;
		structure.transform.localPosition = structurePosition;
		structure.GetComponent<Entity>().affinity = gameObject;
		structure.GetComponent<Entity>().reversePosition = reversePosition;
		structuresInPlay.Add (structure);
	}
	
	void AddStructures(){
		int index = 0;
		foreach(GameObject structure in structureList){
			Vector3 position = new Vector3((float)structurePositions[index], 0, 0);
			AddStructure (position, structure);
			index++;
		}
	}
	
	public bool PlayerCanRespawn(){
		return(currentPlayerRespawnTime >= maxPlayerRespawnTime);
	}
	
	private void ResetPlayerRespawn(){
		currentPlayerRespawnTime = 0;
	}
	
	public void ReceiveDamage(float damage){
		currentHealth -= damage;
	}
	
	public float CurrentHealthRatio(){
		return(currentHealth / maxHealth);
	}
}
