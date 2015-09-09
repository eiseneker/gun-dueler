using UnityEngine;
using System.Collections;

public class CoreMeter : MonoBehaviour {
	public int playerNumber;
	
	private Core core;
	private float meterRatio;
	private Transform filler;
	private Transform dangerText;
	private Transform coreText;	
	
	void Start () {
		filler = transform.Find ("Filler");
		dangerText = transform.Find ("Danger Text");
		coreText = transform.Find ("Core Text");
	}
	
	void Update () {
		if(core == null) {
			core = GetCore();
			meterRatio = 1;
		}else{
			meterRatio = core.CurrentHealthRatio();
			dangerText.gameObject.SetActive(meterRatio <= 0.1f);
			coreText.gameObject.SetActive(meterRatio > 0.1f);
		}
		filler.localScale = new Vector3(meterRatio, 1, 1);
	}
	
	private Core GetCore() {
		GameObject coreObject = GameObject.Find ("Player "+playerNumber+" Fleet/Core(Clone)");
		if(coreObject){
			return(coreObject.GetComponent<Core>());
		}else{
			return(null);
		}
	}
}
