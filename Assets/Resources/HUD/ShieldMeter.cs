using UnityEngine;
using System.Collections;

public class ShieldMeter : MonoBehaviour {
	public int playerNumber;
	
	private Shield shield;
	private float meterRatio;
	private Transform filler;
	private Transform shieldText;
	private Transform brokenText;

	// Use this for initialization
	void Start () {
		filler = transform.Find ("Filler");
		shieldText = transform.Find ("Shield Text");
		brokenText = transform.Find ("Broken Text");
	}
	
	// Update is called once per frame
	void Update () {
		if(shield == null) {
			shield = GetShield();
			meterRatio = 1;
		}else{
			meterRatio = shield.CurrentHealthRatio();
			shieldText.gameObject.SetActive(!shield.ShieldIsBroken());
			brokenText.gameObject.SetActive(shield.ShieldIsBroken());
		}
		filler.localScale = new Vector3(meterRatio, 1, 1);
	}
	
	private Shield GetShield() {
		GameObject shieldObject = GameObject.Find ("Player "+playerNumber+" Fleet/Player(Clone)/Shield(Clone)");
		if(shieldObject){
			return(shieldObject.GetComponent<Shield>());
		}else{
			return(null);
		}
	}
}
