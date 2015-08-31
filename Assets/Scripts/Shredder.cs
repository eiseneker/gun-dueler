using UnityEngine;
using System.Collections;

public class Shredder : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D collision) {
		Projectile bullet = collision.gameObject.GetComponent<Projectile>();
		if(bullet){
			collision.gameObject.GetComponent<Projectile>().DestroyMe();
		}
	}
}
