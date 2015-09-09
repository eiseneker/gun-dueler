using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnTurret : MonoBehaviour {
	private float spawnsPerSecond = 0.5f;
	private int[] spawnLevelProbabilities = { 0, 1, 1, 1 };
	
	public GameObject[] minionPrefabs;
	
	void Start(){
	}
	
	void Update () {
		if(GameController.gameStarted){
			float probability = spawnsPerSecond * Time.deltaTime;
			
			if(Random.value < probability){
				SpawnMinion ();
			}
		}
	}
	
	void SpawnMinion(){
		Quaternion rotation = new Quaternion();
		int level = spawnLevelProbabilities[Random.Range (0, spawnLevelProbabilities.Length)];
		GameObject minion = Instantiate (minionPrefabs[level], transform.position, rotation) as GameObject;
		minion.transform.parent = transform;
		print (minion.GetComponent<Entity>());
		minion.transform.Find ("Ship").GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
		if(GetComponent<Entity>().reversePosition){
			minion.transform.rotation = Quaternion.Euler(0f, 0f, 180);
		}
	}
}
