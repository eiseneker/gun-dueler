using UnityEngine;
using System.Collections;

public class PowerUpPosition : Position {

	private bool isTaken = false;
	
	void OnTriggerEnter2D(Collider2D collider){
		PowerUp powerUp = collider.GetComponent<PowerUp>();
		if(powerUp){
			powerUp.SetPowerUpPosition(gameObject);
		}
	}
	
	public void TakePosition(){
		isTaken = true;
		PowerUpController.openPowerUpPositions.Remove(this);
	}
	
	public void ReleasePosition(){
		isTaken = false;
		PowerUpController.openPowerUpPositions.Add(this);
	}
	
	public bool IsTaken(){
		return(isTaken);
	}

}
