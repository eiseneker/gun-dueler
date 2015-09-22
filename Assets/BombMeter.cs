using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BombMeter : MonoBehaviour {
	public Player player;
	public BombLauncher weapon;
	
	void Start () {
		transform.parent = GameObject.Find ("HUD").transform;
	}
	
	void Update () {
		transform.Find ("Text").GetComponent<Text>().text = weapon.currentBombCount.ToString ();
		transform.position = player.transform.position;
		Camera camera = GameObject.Find ("Cameras/HUD Camera").GetComponent<Camera>();
		
		
		//this is the ui element
		RectTransform UI_Element = this.GetComponent<RectTransform>();
		
		//first you need the RectTransform component of your canvas
		RectTransform CanvasRect=GameObject.Find ("HUD").GetComponent<RectTransform>();
		
		//then you calculate the position of the UI element
		//0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.
		
		Vector2 ViewportPosition = camera.WorldToViewportPoint(player.gameObject.transform.position);
		Vector2 WorldObject_ScreenPosition=new Vector2(
			((ViewportPosition.x*CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x*0.5f)) + 40,
			((ViewportPosition.y*CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y*0.5f)) - 100);
		
		
		UI_Element.anchoredPosition=WorldObject_ScreenPosition;
	}
}
