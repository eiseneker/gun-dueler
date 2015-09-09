using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.Find ("Body").gameObject.activeSelf == false){
			Destroy (gameObject);
		}
	}
}
