using UnityEngine;
using System.Collections;

public class PlayerHUD : MonoBehaviour {
	private Vector3  currentVelocity = new Vector3(0, 0, 0);
	private Camera camera;
	
	public Player player;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find ("Cameras/Joined Camera").GetComponent<Camera>();
		transform.Find ("Health Meter").GetComponent<HealthMeter>().player = player;
		transform.Find ("Ex Meter").GetComponent<ExMeter>().player = player;
		transform.Find ("Bomb Meter").GetComponent<BombMeter>().weapon = player.bombLauncher;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(player == null){
			Destroy(gameObject);
		}else{
			Vector3 newPosition = new Vector3(player.transform.position.x - .25f, player.transform.position.y - 0.75f, player.transform.position.z);
			transform.position = camera.WorldToScreenPoint(newPosition);
		}
	}
}
