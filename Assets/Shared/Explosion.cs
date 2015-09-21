using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public bool hazardous = false;

	void Start () {
	}
	
	void Update () {
		if(transform.Find ("Body").gameObject.activeSelf == false){
			Destroy (gameObject);
		}
	}
	
	void OnTriggerEnter2D(Collider2D collider){
		if(hazardous){
			IHarmable harmedObject = collider.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
			if(harmedObject != null){
				harmedObject.ReceiveHit(50, gameObject, gameObject);
			}
		}
	}
}
