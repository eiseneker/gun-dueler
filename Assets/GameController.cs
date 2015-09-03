using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	
	public static bool gameStarted;
	private float gameTimer = 3f;
	
	void Start(){
		gameStarted = false;
	}
	
	void Update(){
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
	
	}
	
	private void StartGame(){
		Transform playerSpace = GameObject.Find ("Game Root/Players").transform;
		foreach(Transform fleet in playerSpace){
			fleet.gameObject.SetActive(true);
		}
		gameStarted = true;
	}
	
	public static void LoadWinScreen(){
		Player.players.Clear ();
		Application.LoadLevel("Win");
	}
	
}