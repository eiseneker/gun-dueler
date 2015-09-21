using UnityEngine;
using System.Collections;

public class Bull : Minion {
	private GameObject enemyPlayer;
	private VehicleControls vehicleControls;
	
	public override void Start(){
		base.Start();
		speed = 2;
		enemyPlayer = GetComponent<Entity>().EnemyPlayer();
		vehicleControls = GetComponent<VehicleControls>();
	}

	public override void Update(){
		base.Update();
		speed = 5;
		if(enemyPlayer){
			GetComponent<Entity>().FaceObject (enemyPlayer);
		}
		vehicleControls.Accelerate();
	}
	
	private void OnCollisionEnter2D(Collision2D collision){
		if(collision.gameObject.GetComponent<Entity>().affinity != GetComponent<Entity>().affinity){
			IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
			if(harmedObject != null){
				harmedObject.ReceiveHit(1, gameObject, gameObject);
			}
			DestroyMe();
		}
	}
}
