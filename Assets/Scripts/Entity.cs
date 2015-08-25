using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	public GameObject affinity;
	public bool reversePosition;
	
	void Start(){
		SetRotation ();
	}
	
	void SetRotation(){
		if(GetComponent<Entity>().reversePosition && transform.rotation.z == 0){
			gameObject.transform.eulerAngles = new Vector3(
				gameObject.transform.eulerAngles.x,
				gameObject.transform.eulerAngles.y,
				gameObject.transform.eulerAngles.z + 180);
		}
	}
}
