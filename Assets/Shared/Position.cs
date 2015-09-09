using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

	public int level;

	//develompent only

	void OnDrawGizmos (){
		Gizmos.DrawWireSphere(transform.position, 1);
	}
}
