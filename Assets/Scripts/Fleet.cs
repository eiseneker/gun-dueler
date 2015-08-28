using UnityEngine;
using System.Collections;

public class Fleet : MonoBehaviour {

	public GameObject flagShipPrefab;
	public GameObject[] minionFormations;
	public Color teamColor;
	public float width;
	public float height;
	public int playerNumber;
	
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
	
	void AddFlagShip(){
		flagShip = Instantiate (flagShipPrefab, transform.Find ("Player Position").position, Quaternion.identity) as GameObject;
		flagShip.transform.parent = transform;
		flagShip.GetComponent<Entity>().affinity = gameObject;
		flagShip.GetComponent<Entity>().reversePosition = GetComponent<Entity>().reversePosition;
		flagShip.GetComponent<Player>().SetPlayerNumber(playerNumber);
	}
	
	void AddMinionFormation(){
		GameObject minionFormation  = Instantiate(minionFormations[0], transform.Find ("Formation Position").position, Quaternion.identity) as GameObject;
		minionFormation.transform.parent = minions.transform;
		minionFormation.GetComponent<Entity>().affinity = affinity;
		minionFormation.GetComponent<Entity>().reversePosition = reversePosition;
	}
}
