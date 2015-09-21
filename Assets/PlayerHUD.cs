using UnityEngine;
using System.Collections;

public class PlayerHUD : MonoBehaviour {

	public int playerNumber;

	// Use this for initialization
	void Start () {
		transform.Find ("Engine Meter").GetComponent<EngineMeter>().playerNumber = playerNumber;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
