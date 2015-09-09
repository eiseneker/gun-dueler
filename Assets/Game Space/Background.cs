using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	float pos = 0;


	// Use this for initialization
	void Start () {
		
	}
	
	void Update () {
		pos += .004f;
		
		Transform quad = transform.Find ("Gravel");
		quad.renderer.material.mainTextureOffset = new Vector2(0, pos);
	}
}
