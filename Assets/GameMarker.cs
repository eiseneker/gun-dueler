using UnityEngine;
using System.Collections;

public class GameMarker : MonoBehaviour {

	public Rigidbody2D myRigidbody;
	
	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameController.gameStarted){
			GetComponent<Rigidbody2D>().AddForce (Vector3.right * 150 * Time.deltaTime);
			GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, 18);
		}
	}
}
