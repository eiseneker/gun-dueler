using UnityEngine;
using System.Collections;

public class BombLauncher : MonoBehaviour {
	public Player player;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Fire(){
		print (player);
		GameObject bombObject = Instantiate (Resources.Load ("Bomb"), transform.position, Quaternion.identity) as GameObject;
		Bomb bomb = bombObject.GetComponent<Bomb>();
		bomb.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity;	
		bomb.owner = player;
	}
}
