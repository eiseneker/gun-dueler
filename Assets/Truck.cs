using UnityEngine;
using System.Collections;

public class Truck : MonoBehaviour {

	private Rigidbody2D myRigidbody;
	private float accelerationFactor = 50;
	private float brakeFactor = 50;
	private float maxVelocity = 7;
	private float minVelocity = 3;
	private float idleVelocity = 5;
	private float idleFactor = 25;
	private float speed = 1;
	private ArrayList structurePositions = new ArrayList();
	private ArrayList structureList = new ArrayList();
	private float structureSpacing = 2;
	
	public bool reversePosition;
	public GameObject[] structures;
	public static ArrayList trucks = new ArrayList();
	public ArrayList structuresInPlay = new ArrayList();
	public GameObject headElement;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
		
		structureList.AddRange (structures);
		
		for(int i = 0 - structures.Length/2; i < structures.Length/2; i++){
			structurePositions.Add (i * structureSpacing);
		}
		
		AddStructures();
		headElement = structuresInPlay[structuresInPlay.Count - 1] as GameObject;
		trucks.Add(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		Accelerate (1);
	}
	
	private void Accelerate(float movement){
		myRigidbody.AddRelativeForce (Vector3.up * accelerationFactor * movement * Time.deltaTime * speed);
		myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, maxVelocity);
	}
	
	
	void AddStructure(Vector3 structurePosition, GameObject structurePrefab){
		GameObject structure = Instantiate (structurePrefab, Vector3.zero, Quaternion.identity) as GameObject;
		structure.transform.parent = transform;
		structure.transform.localPosition = structurePosition;
		structure.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
		structure.GetComponent<Entity>().reversePosition = reversePosition;
		structuresInPlay.Add (structure);
	}
	
	void AddStructures(){
		int index = 0;
		foreach(GameObject structure in structureList){
			Vector3 position = new Vector3(0, (float)structurePositions[index], 0);
			AddStructure (position, structure);
			index++;
		}
	}
}
