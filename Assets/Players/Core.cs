using UnityEngine;
using System.Collections;

public class Core : MonoBehaviour, IHarmable {

	private float currentHealth;
	private float maxHealth = 5;
	public int enemyPlayerNumber = 0;

	void Start () {
		currentHealth = maxHealth;
		transform.Find ("Sentry").GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
		Transform body = transform.Find ("Body");
		body.GetComponent<SpriteRenderer>().color = GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor;
	}
	
	void Update(){
		if(enemyPlayerNumber == 0){
			FetchEnemyPlayer();
		}
	}
	
	public void ReceiveHit(float damage, GameObject attackerObject) {
		currentHealth -= damage;
		IAttacker attacker = attackerObject.GetComponent(typeof(IAttacker)) as IAttacker;
		attacker.RegisterSuccessfulAttack(5);
		if(currentHealth <= 0){
			DestroyMe ();
		}
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

