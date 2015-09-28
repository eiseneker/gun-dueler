using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public bool hazardous = false;
	public float currentHazardousFrames;
	public float maxHazardousFrames = 0.05f;
	public float startHazardousFrames = 0.03f;

	void Start () {
	}
	
	void Update () {
		currentHazardousFrames += Time.deltaTime;
		if(currentHazardousFrames <= startHazardousFrames && currentHazardousFrames >= maxHazardousFrames + startHazardousFrames){
			GetComponent<Collider2D>().enabled = false;
		}else{
			GetComponent<Collider2D>().enabled = true;
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
