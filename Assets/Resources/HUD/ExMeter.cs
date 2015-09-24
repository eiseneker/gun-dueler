using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExMeter : MonoBehaviour {
	public Player player;
	
	private float meterRatio;
	private Transform filler;
	
	void Start () {
		filler = transform.Find ("Filler");
		meterRatio = 0;
	}
	
	void Update () {
		meterRatio = player.CurrentExRatio();
		filler.GetComponent<Image>().fillAmount = meterRatio;
	}
}
