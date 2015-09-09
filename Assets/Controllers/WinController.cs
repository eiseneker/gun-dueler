using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find ("Title").GetComponent<Text>().text = "Player " + StateController.lastWinner + " wins!";
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis ("Player1_PrimaryWeapon") == 1 || Input.GetAxis ("Player2_PrimaryWeapon") == 1){
			LoadGame ();
		}
	}
	
	public void LoadGame(){
		Application.LoadLevel ("Game");
	}
}
