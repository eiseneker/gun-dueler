using UnityEngine;
using System.Collections;

public class Truck : MonoBehaviour {

	private ArrayList structurePositions = new ArrayList();
	private ArrayList structureList = new ArrayList();
	private float structureSpacing = 2;
	private VehicleControls vehicleControls;
	
	public bool reversePosition;
	public GameObject[] structures;
	public static ArrayList trucks = new ArrayList();
	public ArrayList structuresInPlay = new ArrayList();
	public GameObject headElement;
	public GameObject lastElement;

	// Use this for initialization
	void Start () {
		structureList.AddRange (structures);
		
		for(int i = 0 - structures.Length/2; i < structures.Length/2; i++){
			structurePositions.Add (i * structureSpacing);
		}
		
		AddStructures();
		headElement = structuresInPlay[structuresInPlay.Count - 1] as GameObject;
		lastElement = structuresInPlay[0] as GameObject;
		trucks.Add(gameObject);
		vehicleControls = GetComponent<VehicleControls>();
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
