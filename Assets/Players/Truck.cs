using UnityEngine;
using System.Collections;

public class Truck : MonoBehaviour {

	private ArrayList structurePositions = new ArrayList();
	private ArrayList structureList = new ArrayList();
	private float structureSpacing = 2;
	private Rigidbody2D myRigidbody;
	
	public bool reversePosition;
	public GameObject[] structures;
	public static ArrayList trucks = new ArrayList();
	public ArrayList structuresInPlay = new ArrayList();
	public GameObject headElement;
	public GameObject lastElement;
	public VehicleControls vehicleControls;

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
	}
	
	// Update is called once per frame
	void Update () {
		vehicleControls.Accelerate ();
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
	
	public Vector3 Velocity(){
		return(myRigidbody.velocity);
	}
	
	
}
