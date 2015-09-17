using UnityEngine;
using System.Collections;

public class CoreMeter : MonoBehaviour {
	public int playerNumber;
	
	private Fleet fleet;
	private float meterRatio;
	private Transform filler;
//	private Transform dangerText;
//	private Transform coreText;	
	
	void Start () {
		filler = transform.Find ("Filler");
//		dangerText = transform.Find ("Danger Text");
//		coreText = transform.Find ("Core Text");
	}
	
	void Update () {
		if(fleet == null) {
			fleet = GetFleet();
			meterRatio = 1;
		}else{
			meterRatio = fleet.CurrentHealthRatio();
//			dangerText.gameObject.SetActive(meterRatio <= 0.1f);
//			coreText.gameObject.SetActive(meterRatio > 0.1f);
		}
		filler.localScale = new Vector3(meterRatio, 1, 1);
	}
	
	private Fleet GetFleet() {
		GameObject fleetObject = GameObject.Find ("Player "+playerNumber+" Fleet");
		if(fleetObject){
			return(fleetObject.GetComponent<Fleet>());
		}else{
			return(null);
		}
	}
}
