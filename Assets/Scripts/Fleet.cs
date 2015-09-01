using UnityEngine;
using System.Collections;

public class Fleet : MonoBehaviour {

	public GameObject flagShipPrefab;
	public GameObject corePrefab;
	public GameObject[] minionFormations;
	public Color teamColor;
	public float width;
	public float height;
	public int playerNumber;
	
	private GameObject affinity;
	private bool reversePosition;
	private GameObject minions;
	private GameObject flagShip;
	private GameObject core;
	
	private float maxPlayerRespawnTime = 5;
	private float currentPlayerRespawnTime;

	void Start () {
		affinity = gameObject;
		reversePosition = GetComponent<Entity>().reversePosition;
		GetComponent<Entity>().affinity = gameObject;
		
		AddMinionsObject ();
		AddFlagShip();
		AddCore();
		AddMinionFormation (0);
	}
	
	void Update() {
		if(flagShip == null){
			if(currentPlayerRespawnTime >= maxPlayerRespawnTime){
				AddFlagShip ();
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
	
	void AddFlagShip(){
		flagShip = Instantiate (flagShipPrefab, transform.Find ("Player Position").position, Quaternion.identity) as GameObject;
		flagShip.transform.parent = transform;
		flagShip.GetComponent<Entity>().affinity = gameObject;
		flagShip.GetComponent<Entity>().reversePosition = GetComponent<Entity>().reversePosition;
		flagShip.GetComponent<Player>().SetPlayerNumber(playerNumber);
		currentPlayerRespawnTime = 0;
		
	}
	
	void AddCore(){
		core = Instantiate (corePrefab, transform.Find ("Core Position").position, Quaternion.identity) as GameObject;
		core.transform.parent = transform;
		core.GetComponent<Entity>().affinity = gameObject;
	}
	
	public void AddMinionFormation(int index){
		GameObject minionFormation  = Instantiate(minionFormations[index], transform.Find ("Formation Position").position, Quaternion.identity) as GameObject;
		minionFormation.transform.parent = minions.transform;
		minionFormation.GetComponent<Entity>().affinity = affinity;
		minionFormation.GetComponent<Entity>().reversePosition = reversePosition;
	}
}
