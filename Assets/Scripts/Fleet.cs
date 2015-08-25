using UnityEngine;
using System.Collections;

public class Fleet : MonoBehaviour {

	public GameObject flagShipPrefab;
	public GameObject[] minionFormations;
	public Color teamColor;
	
	private GameObject affinity;
	private bool reversePosition;
	private GameObject minions;
	private GameObject flagShip;

	void Start () {
		affinity = gameObject;
		reversePosition = GetComponent<Entity>().reversePosition;
		GetComponent<Entity>().affinity = gameObject;
		
		AddMinionsObject ();
		AddFlagShip();
		AddMinionFormation ();
	}
	
	void AddMinionsObject(){
		minions = new GameObject("Minions");
		Instantiate (minions, transform.position, Quaternion.identity);	
		minions.transform.parent = transform;
	}
	
	void AddFlagShip(){
		flagShip = Instantiate (flagShipPrefab, transform.position, Quaternion.identity) as GameObject;
		flagShip.transform.parent = transform;
		flagShip.GetComponent<Entity>().affinity = gameObject;
		flagShip.GetComponent<Entity>().reversePosition = GetComponent<Entity>().reversePosition;
	}
	
	void AddMinionFormation(){
		GameObject minionFormation  = Instantiate(minionFormations[0], transform.position, Quaternion.identity) as GameObject;
		minionFormation.transform.parent = minions.transform;
		minionFormation.GetComponent<Entity>().affinity = affinity;
		minionFormation.GetComponent<Entity>().reversePosition = reversePosition;
	}
}
