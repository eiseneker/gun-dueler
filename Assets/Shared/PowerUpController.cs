using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour {
	public static int activePowerUpCount = 0;
	
	public GameObject powerUpPrefab;
	
	private float spawnsPerSecond = 1f;
	
	private int[] spawnLevelProbabilities = { 1, 2, 2, 2 };
	
	void Update () {
		if(LevelController.gameStarted && activePowerUpCount < 1){
			float probability = spawnsPerSecond * Time.deltaTime;
			
			if(Random.value < probability){
				SpawnPowerUp ();
			}
		}
	}
	
	void SpawnPowerUp(){
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
