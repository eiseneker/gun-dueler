using UnityEngine;
using System.Collections;

public class ExMeter : MonoBehaviour {
	public int playerNumber;
	
	private Player player;
	private float meterRatio;
	private Transform filler;
	private Transform exText;
	private Transform superText;

	void Start () {
		filler = transform.Find ("Filler");
		exText = transform.Find ("Ex Text");
		superText = transform.Find ("Super Text");
	}
	
	void Update () {
		if(player == null) {
			player = GetPlayer();
			meterRatio = 1;
		}else{
			meterRatio = player.CurrentExRatio();
			exText.gameObject.SetActive(player.IsInExMode () || (meterRatio < 1 && meterRatio >= 0.5f));
			superText.gameObject.SetActive(meterRatio >= 1);
		}
		filler.localScale = new Vector3(meterRatio, 1, 1);
	}
	
	private Player GetPlayer() {
		GameObject playerObject = GameObject.Find ("Player "+playerNumber+" Fleet/Player(Clone)");
		if(playerObject){
			return(playerObject.GetComponent<Player>());
		}else{
			return(null);
		}
	}
}
