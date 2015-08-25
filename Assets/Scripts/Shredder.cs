using UnityEngine;
using System.Collections;

public class Shredder : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D collision) {
		Bullet bullet = collision.gameObject.GetComponent<Bullet>();
		if(bullet){
			collision.gameObject.GetComponent<Bullet>().DestroyMe();
		}
	}
}
