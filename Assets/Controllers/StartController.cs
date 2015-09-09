using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartController : MonoBehaviour {
	
	public void LoadGame(){
		Application.LoadLevel ("Game");
	}
	
	void Update(){
		if(Input.GetAxis ("Player1_PrimaryWeapon") == 1 || Input.GetAxis ("Player2_PrimaryWeapon") == 1){
			LoadGame ();
		}
	}
	
}
