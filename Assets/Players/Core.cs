using UnityEngine;
using System.Collections;

public class Core : Agent {

	public int enemyPlayerNumber = 0;

	private DamageBehavior damageBehavior;

	void Start () {
		damageBehavior = GetComponent<DamageBehavior>();
		transform.Find ("Sentry").GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
		transform.Find ("Body").GetComponent<SpriteRenderer>().color = GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor;
	}
	
	void Update(){
		if(enemyPlayerNumber == 0){
			FetchEnemyPlayer();
		}
	}
	
	public override void ReceiveHit(float damage, GameObject attackerObject) {
		IAttacker attacker = ResolveAttacker(attackerObject);
		if(attacker != null) attacker.RegisterSuccessfulAttack(0);
		damageBehavior.ReceiveDamage(damage);
		if(CurrentHealthRatio() <= 0){
			DestroyMe ();
		}
	}
	
	public float CurrentHealthRatio(){
		return(damageBehavior.CurrentHealthRatio());
	}
	
	private void DestroyMe(){
		Destroy (gameObject);
		StateController.lastWinner = enemyPlayerNumber;
		GameController.LoadWinScreen();
	}
	
	private void FetchEnemyPlayer(){
		GameObject enemyPlayer = GetComponent<Entity>().EnemyPlayer();
		if(enemyPlayer){
			enemyPlayerNumber = enemyPlayer.GetComponent<Entity>().affinity.GetComponent<Fleet>().playerNumber;
		}
	}
}

