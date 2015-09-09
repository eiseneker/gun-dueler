using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	void Start () {
	}
	
	void Update () {
		if(transform.Find ("Body").gameObject.activeSelf == false){
			Destroy (gameObject);
		}
	}
}
