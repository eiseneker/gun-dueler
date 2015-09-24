using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {
	
	void OnCollisionEnter2D(Collision2D collision){
		IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
		if(harmedObject != null){
			harmedObject.ReceiveHit(10, gameObject, gameObject);
		}
	}
}
