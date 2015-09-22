using UnityEngine;
using System.Collections;

public class PlayerHUD : MonoBehaviour {
	private Vector3  currentVelocity = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Player player = Player.players[0].GetComponent<Player>();
		Camera camera = GameObject.Find ("Cameras/Joined Camera").GetComponent<Camera>();
		Vector3 newPosition = new Vector3(player.transform.position.x - .25f, player.transform.position.y - 0.75f, player.transform.position.z);
		transform.position = camera.WorldToScreenPoint(newPosition);
	}
}
