using UnityEngine;
using System.Collections;

public class BombLauncher : Weapon {
	private int maxBombCount = 3;
	public int currentBombCount;
	private GameObject bombMeter;
	
	public BombLauncher() {
		fireDelay = 3f;
		currentBombCount = maxBombCount;
	}
	
	public override void Start(){
		base.Start ();
		if(bombMeter == null){
			bombMeter = Instantiate ( Resources.Load ("HUD/Bomb Meter"), transform.position, Quaternion.identity) as GameObject;
			bombMeter.GetComponent<BombMeter>().player = player;
			bombMeter.GetComponent<BombMeter>().weapon = this;
		}	
	}

	public void Fire(){
		if(CanFire () && currentBombCount > 0){
			GameObject bombObject = Instantiate (Resources.Load ("Bomb"), transform.position, Quaternion.identity) as GameObject;
			Bomb bomb = bombObject.GetComponent<Bomb>();
			bomb.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity;	
			bomb.owner = player;
			timeSinceLastFire = 0;
			currentBombCount--;
		}
	}
}
