using UnityEngine;
using System.Collections;

public class PlayerHUD : MonoBehaviour {
	private Vector3 currentVelocity = new Vector3(0, 0, 0);
	private Camera camera;
	
	public Player player;

	// Use this for initialization
	void Start () {
		transform.Find ("Health Meter").GetComponent<HealthMeter>().player = player;
		transform.Find ("Ex Meter").GetComponent<ExMeter>().player = player;
		transform.Find ("Bomb Meter").GetComponent<BombMeter>().weapon = player.bombLauncher;
		transform.localScale *= 0.025f;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(player == null){
			Destroy(gameObject);
		}else{
			transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z);
		}
	}
}
