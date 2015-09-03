using UnityEngine;
using System.Collections;

public class Shredder : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D collision) {
		IShreddable shreddable = collision.gameObject.GetComponent(typeof(IShreddable)) as IShreddable;
		if(shreddable != null){
			shreddable.DestroyMe();
		}
	}
}
