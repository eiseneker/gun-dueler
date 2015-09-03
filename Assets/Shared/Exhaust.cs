using UnityEngine;
using System.Collections;

public class Exhaust : MonoBehaviour {

	private SpriteRenderer sprite;
	private float flickerInterval = 0.05f;
	private float currentFlickerTimer;
	private Color appliedColor;
	private bool isAppliedColorSet = false;
	private bool isColorChanged = false;

	// Use this for initialization
	void Start () {
		sprite = transform.Find ("Body").GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		Color tempColor = sprite.color;
		tempColor.a = 1;
		currentFlickerTimer += Time.deltaTime;
		if(currentFlickerTimer >= flickerInterval){
			tempColor.a = 0;
			currentFlickerTimer = 0;
		}
		sprite.color = tempColor;
		
		if(isAppliedColorSet && !isColorChanged){
			if(sprite){
				tempColor.r = appliedColor.r;
				tempColor.g = appliedColor.g;
				tempColor.b = appliedColor.b;
				
				sprite.color = tempColor;
				isColorChanged = true;
			}
		}
	}
	
	public void SetColor (Color color){
		appliedColor = color;
		isAppliedColorSet = true;
	}
}
