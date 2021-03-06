﻿using UnityEngine;
using System.Collections;

public class Truck : MonoBehaviour {

	private ArrayList structurePositions = new ArrayList();
	private ArrayList structureList = new ArrayList();
	private float structureSpacing = 3;
	public Rigidbody2D myRigidbody;
	private float reverseFactor = 1;
	
	public bool reversePosition;
	public GameObject[] structures;
	public static ArrayList trucks = new ArrayList();
	public ArrayList structuresInPlay = new ArrayList();
	public GameObject headElement;
	public GameObject lastElement;
	public VehicleControls vehicleControls;
	private GameObject enemyTruck;

	// Use this for initialization
	void Start () {
		structureList.AddRange (structures);
		
		for(int i = 0; i < structures.Length; i++){
			structurePositions.Add (i * structureSpacing);
		}
		
		AddStructures();
		headElement = structuresInPlay[structuresInPlay.Count - 1] as GameObject;
		lastElement = structuresInPlay[0] as GameObject;
		trucks.Add(gameObject);
		vehicleControls = GetComponent<VehicleControls>();
		myRigidbody = GetComponent<Rigidbody2D>();
		transform.Find ("Body").localScale = new Vector3(1, (structuresInPlay.Count * 2), 0);
		transform.Find ("Body").localPosition = new Vector3(0, structuresInPlay.Count - 1, 0);
		if(!reversePosition) reverseFactor = -1;
		GameObject body = transform.Find ("Body").gameObject;
		body.GetComponent<MeshRenderer>().material.color = GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor;
	}
	
	// Update is called once per frame
	void Update () {
		if(enemyTruck == null) enemyTruck = GetEnemyTruck();
		if(headElement.transform.position.x <= enemyTruck.GetComponent<Truck>().lastElement.transform.position.x){
			myRigidbody.velocity = enemyTruck.GetComponent<Truck>().myRigidbody.velocity;
		}else{
			vehicleControls.Accelerate ();
		}
	}
	
	void AddStructure(Vector3 structurePosition, GameObject structurePrefab){
		GameObject structure = Instantiate (structurePrefab, Vector3.zero, Quaternion.identity) as GameObject;
		structure.transform.parent = transform;
		structure.transform.localPosition = structurePosition;
		structure.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
		structure.GetComponent<Entity>().reversePosition = reversePosition;
		structure.GetComponent<Structure>().truck = this;
		structuresInPlay.Add (structure);
		Structure.structureCount++;
	}
	
	void AddStructures(){
		int index = 0;
		foreach(GameObject structure in structureList){
			Vector3 position = new Vector3(0, (float)structurePositions[index], 0);
			AddStructure (position, structure);
			index++;
		}
	}
	
	private GameObject GetEnemyTruck(){
		foreach(GameObject truck in Truck.trucks){
			if(truck != gameObject) {
				return(truck);
			}
		}
		return(null);
	}
	
	public Vector3 Velocity(){
		return(myRigidbody.velocity);
	}
	
	
}
