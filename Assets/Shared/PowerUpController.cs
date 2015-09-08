using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpController : MonoBehaviour {
	public static int activePowerUpCount = 0;
	
	public GameObject powerUpPrefab;
	
	private float spawnsPerSecond = 0.1f;
	private int[] spawnLevelProbabilities = { 1, 2, 2, 2 };
	private float[] rotations = { 1, 0 };
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
		if(GameController.gameStarted && activePowerUpCount < 1){
			float probability = spawnsPerSecond * Time.deltaTime;
			
			if(Random.value < probability){
				SpawnPowerUp ();
			}
		}
	}
	
	void SpawnPowerUp(){
		Quaternion rotation = new Quaternion();
		rotation.z = rotations[Random.Range (0, rotations.Length)];
		GameObject powerUp = Instantiate (powerUpPrefab, transform.position, rotation) as GameObject;
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
