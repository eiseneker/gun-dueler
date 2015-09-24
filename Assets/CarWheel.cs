using UnityEngine;
using System.Collections;

public class CarWheel : MonoBehaviour {

	private Transform body;
	
	public VehicleControls vehicleControls;

	// Use this for initialization
	void Start () {
		body = transform.Find ("Body");
	}
	
	// Update is called once per frame
	void Update () {
		body.Rotate (new Vector3(0, 0, vehicleControls.myRigidbody.velocity.x * -90) * Time.deltaTime);
	}
}
