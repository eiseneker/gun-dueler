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
	public bool reversePosition;
	public Truck truck;

	private float maxHealth = 50;
	private float currentHealth;	
	private GameObject minions;
	private GameObject core;
	private float maxPlayerRespawnTime = 5;
	private float currentPlayerRespawnTime;

	void Start () {
		
		
		AddMinionsObject ();
		AddPlayer();
		AddTruck();
	}
	
	void Update() {
		if(player == null){
			if(currentPlayerRespawnTime < maxPlayerRespawnTime){
				currentPlayerRespawnTime += Time.deltaTime;
			}else{
				AddPlayer ();
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
	
	public GameObject AddPlayer(){
		player = Instantiate (playerPrefab, transform.position, Quaternion.identity) as GameObject;
		player.transform.parent = transform;
		player.GetComponent<Entity>().affinity = gameObject;
		player.GetComponent<Player>().SetPlayerNumber(playerNumber);
		currentPlayerRespawnTime = 0;
		ResetPlayerRespawn();
		return(player);
	}
	
	public void AddTruck(){
		GameObject truckObject = Instantiate (Resources.Load ("Truck"), transform.position, Quaternion.identity) as GameObject;
		truckObject.transform.parent = transform;
		truck = transform.Find ("Truck(Clone)").GetComponent<Truck>();
		truck.structures = structures;
		
		currentHealth = maxHealth;
		
		GetComponent<Entity>().affinity = gameObject;
		truck.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
		truck.reversePosition = reversePosition;
		OrientationHelper.RotateTransform(truck.transform, 270);
		if(reversePosition){
			truck.transform.Translate (Vector3.right * .5f);
		}else{
			truck.transform.Translate (Vector3.left * .5f);
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
