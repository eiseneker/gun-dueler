using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

	public static bool gameStarted = false;
	private float gameTimer = 3f;
	

	public void LoadGame(){
		Application.LoadLevel ("Game");
	}
	
	void Update(){
		if(Application.loadedLevelName == "Game"){
			if(gameStarted == false){
				if(gameTimer > 0){
					gameTimer -= Time.deltaTime;
					foreach(Transform text in GameObject.Find ("HUD/Game Countdown/").transform){
						text.GetComponent<Text>().text = Mathf.CeilToInt(gameTimer).ToString ();
					}
				}else{
					foreach(Transform text in GameObject.Find ("HUD/Game Countdown/").transform){
						text.gameObject.SetActive(false);
					}
					StartGame();
				}
			}
		}else{
			if(Input.GetAxis ("Player1_PrimaryWeapon") == 1 || Input.GetAxis ("Player2_PrimaryWeapon") == 1){
				LoadGame ();
			}
		}
	}
	
	private void StartGame(){
		print ("start the game!");
		Transform playerSpace = GameObject.Find ("Game Root/Players").transform;
		foreach(Transform fleet in playerSpace){
			fleet.gameObject.SetActive(true);
		}
		gameStarted = true;
	}
	
}
