﻿using UnityEngine;
using System.Collections;

public class MagnetMissile : Weapon {
	private float defaultSpeed = 8;
	private float speed;

	private GameObject magnetPrefab;
	
	public MagnetMissile() {
		fireDelay = 3f;
		magnetPrefab = Resources.Load ("magnet") as GameObject;
	}
	
	public void Fire (bool exAttempt) {
		if(CanFire ()){
			bool ex = exAttempt;
			MagnetProjectile magnet;
			speed = defaultSpeed;
		
			if(ex){
				speed *= 1.2f;
				magnet = newProjectile();
				magnet.speed = speed;
				magnet.weapon = this;
				magnet.vector = Vector3.up;
				magnet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
				OrientProjectile(magnet);
				magnet.RotateMe(-90);
				if(player.FacingUp() && player.transform.position.x > player.GetComponent<Entity>().EnemyPlayer().transform.position.x){
					magnet.RotateMe(180);
				}
				if(player.FacingDown() && player.transform.position.x < player.GetComponent<Entity>().EnemyPlayer().transform.position.x){
					magnet.RotateMe(180);
				}
				magnet.defaultOrientation = false;
			}
			
			magnet = newProjectile();
			magnet.speed = speed;
			magnet.weapon = this;
			magnet.vector = Vector3.up;
			magnet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
			OrientProjectile(magnet);
			
			timeSinceLastFire = 0f;
		}
	}
	
	private MagnetProjectile newProjectile(){
		GameObject magnetObject = Instantiate(magnetPrefab, transform.position, Quaternion.identity) as GameObject;
		MagnetProjectile magnet = magnetObject.GetComponent<MagnetProjectile>();
		return(magnet);
	}
}
