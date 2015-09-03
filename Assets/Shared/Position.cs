using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

	

	//develompent only

	void OnDrawGizmos (){
		Gizmos.DrawWireSphere(transform.position, 1);
	}
}
