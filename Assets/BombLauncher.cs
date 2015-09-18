using UnityEngine;
using System.Collections;

public class BombLauncher : Weapon {
	public BombLauncher() {
		fireDelay = 3f;
	}


	public void Fire(){
		if(CanFire ()){
			print ("fire!");
			GameObject bombObject = Instantiate (Resources.Load ("Bomb"), transform.position, Quaternion.identity) as GameObject;
			Bomb bomb = bombObject.GetComponent<Bomb>();
			bomb.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity;	
			bomb.owner = player;
			timeSinceLastFire = 0;
		}
	}
}
