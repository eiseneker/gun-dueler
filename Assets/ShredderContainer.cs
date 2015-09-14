using UnityEngine;
using System.Collections;

public class ShredderContainer : MonoBehaviour {

	private static float highestX;
	
	void Update(){
		transform.position = new Vector3(highestX, 0, 0);
	}

	public static void ReportPosition(float x){
		if(x > highestX){
			highestX = x;
		}
	}
}
