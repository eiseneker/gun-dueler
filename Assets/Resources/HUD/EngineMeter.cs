using UnityEngine;
using System.Collections;

public class EngineMeter : MonoBehaviour {
	public int playerNumber;
	
	private Engine engine;
	private float meterRatio;
	private Transform filler;
	private Transform healthText;
	private Transform criticalText;

	void Start () {
		filler = transform.Find ("Filler");
		criticalText = transform.Find ("Critical Text");
	}
	
	void Update () {
		if(engine == null) {
			engine = GetEngine();
			meterRatio = 1;
		}else{
			meterRatio = engine.damageBehavior.CurrentHealthRatio();
		}
		filler.localScale = new Vector3(meterRatio, 1, 1);
	}
	
	private Engine GetEngine() {
		GameObject engineObject = GameObject.Find ("Game Root/Players/Player "+playerNumber+" Fleet/Truck(Clone)/Engine(Clone)");
		if(engineObject){
			print ("engine found");
			return(engineObject.GetComponent<Engine>());
		}else{
			return(null);
		}
	}
}
