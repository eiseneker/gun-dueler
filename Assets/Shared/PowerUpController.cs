using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpController : MonoBehaviour {
	public static int activePowerUpCount = 0;
	
	public GameObject powerUpPrefab;
	
	private float spawnsPerSecond = 1f;
	
	private int[] spawnLevelProbabilities = { 1, 2, 2, 2 };
	
	public static List<PowerUpPosition> openPowerUpPositions = new List<PowerUpPosition>();
	
	void Start(){
		openPowerUpPositions.Clear ();
		foreach(Transform position in transform.Find("Positions")){
			PowerUpPosition powerUpPosition = position.GetComponent<PowerUpPosition>();
			if(!powerUpPosition.IsTaken()){
				openPowerUpPositions.Add (powerUpPosition);
			}
		}
	}
	
	void Update () {
		print (activePowerUpCount);
		if(GameController.gameStarted && activePowerUpCount < 1){
			float probability = spawnsPerSecond * Time.deltaTime;
			
			if(Random.value < probability){
				SpawnPowerUp ();
			}
		}
	}
	
	void SpawnPowerUp(){
		print ("spawning a power up...");
		GameObject powerUp = Instantiate (powerUpPrefab, transform.position, Quaternion.identity) as GameObject;
		int level = spawnLevelProbabilities[Random.Range (0, spawnLevelProbabilities.Length)];
		powerUp.GetComponent<PowerUp>().level = level;
		powerUp.transform.parent = transform;
		IncrementPowerUps();
	}
	
	public static void IncrementPowerUps(){
		activePowerUpCount++;
	}
	
	public static void DecrementPowerUps(){
		activePowerUpCount--;
	}
}
