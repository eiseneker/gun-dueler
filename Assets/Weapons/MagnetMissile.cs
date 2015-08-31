using UnityEngine;
using System.Collections;

public class MagnetMissile : Weapon {
	private float magnetSpeed = 7;
	private GameObject magnetPrefab;
	
	public MagnetMissile() {
		fireDelay = 3f;
		magnetPrefab = Resources.Load ("magnet") as GameObject;
	}
	
	public void Fire () {
		if(CanFire ()){
			MagnetProjectile magnet = newProjectile();
			magnet.speed = magnetSpeed;
			magnet.weapon = this;
			magnet.vector = Vector3.up;
			magnet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
			RotateProjectile(magnet);
			
			timeSinceLastFire = 0f;
		}
	}
	
	private MagnetProjectile newProjectile(){
		GameObject magnetObject = Instantiate(magnetPrefab, transform.position, Quaternion.identity) as GameObject;
		MagnetProjectile magnet = magnetObject.GetComponent<MagnetProjectile>();
		return(magnet);
	}
}
