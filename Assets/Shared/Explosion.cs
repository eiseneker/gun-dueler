using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public bool hazardous = false;
	public float currentHazardousFrames;
	public float maxHazardousFrames = 0.05f;

	void Start () {
	}
	
	void Update () {
		currentHazardousFrames += Time.deltaTime;
		if(currentHazardousFrames >= maxHazardousFrames){
			GetComponent<Collider2D>().enabled = false;
		}
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
