using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthMeter : MonoBehaviour {
	public Player player;
	
	private float meterRatio;
	private Transform filler;
	
	void Start () {
		filler = transform.Find ("Filler");
		meterRatio = 0;
	}
	
	void Update () {
		meterRatio = player.damageBehavior.CurrentHealthRatio();
		filler.GetComponent<Image>().fillAmount = meterRatio;
	}
}
