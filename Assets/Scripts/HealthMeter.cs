using UnityEngine;
using System.Collections;

public class HealthMeter : MonoBehaviour {
	public int playerNumber;
	
	private Player player;
	private float meterRatio;
	private Transform filler;
	private Transform healthText;
	private Transform criticalText;

	void Start () {
		filler = transform.Find ("Filler");
		healthText = transform.Find ("Health Text");
		criticalText = transform.Find ("Critical Text");
	}
	
	void Update () {
		if(player == null) {
			player = GetPlayer();
			meterRatio = 1;
		}else{
			meterRatio = player.CurrentHealthRatio();
			healthText.gameObject.SetActive(!player.IsCritical());
			criticalText.gameObject.SetActive(player.IsCritical());
		}
		filler.localScale = new Vector3(meterRatio, 1, 1);
	}
	
	private Player GetPlayer() {
		GameObject playerObject = GameObject.Find ("Player "+playerNumber+" Fleet/FlagShip(Clone)");
		if(playerObject){
			return(playerObject.GetComponent<Player>());
		}else{
			return(null);
		}
	}
}
