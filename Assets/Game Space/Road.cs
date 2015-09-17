﻿using UnityEngine;
using System.Collections;

public class Road : MonoBehaviour {

	private LoadMarker loadMarker;
	
	private static ArrayList roads = new ArrayList();

	// Use this for initialization
	void Start () {
		loadMarker = transform.Find ("LoadMarker").GetComponent<LoadMarker>();
		loadMarker.road = this;
		roads.Add (this);
		transform.eulerAngles = new Vector3(
			32,
			transform.eulerAngles.y,
			transform.eulerAngles.z);
	}
	
	
	public void LoadNewRoad(){
		GameObject road = Instantiate (gameObject, new Vector3(transform.position.x + 100, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
		road.transform.parent = transform.parent;
	}
	
	public void UnloadFirstRoad(){
		Road road = roads[0] as Road;
		if(road != this){
			roads.Remove(road);
			Destroy (road.gameObject);
		}
	}
}
