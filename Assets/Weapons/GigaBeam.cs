using UnityEngine;
using System.Collections;

public class GigaBeam : Weapon {
	private GameObject laserPrefab;
	
	public GigaBeam() {
		laserPrefab = Resources.Load ("GigaLaserContainer") as GameObject;
	}
	
	public void Fire () {
		if(CanFire ()){
			GameObject laserObject = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
			GigaBeamProjectile laser = laserObject.transform.Find ("GigaLaser").GetComponent<GigaBeamProjectile>();
			laser.weapon = this;
			laser.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
			RotateProjectile(laser);
			timeSinceLastFire = 0f;
		}
	}
}
