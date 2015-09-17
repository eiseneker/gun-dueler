using UnityEngine;
using System.Collections;

public class PlayerHUD : MonoBehaviour {

	public int playerNumber;

	// Use this for initialization
	void Start () {
		transform.Find ("Health Meter").GetComponent<HealthMeter>().playerNumber = playerNumber;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
