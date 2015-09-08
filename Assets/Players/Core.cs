using UnityEngine;
using System.Collections;

public class Core : MonoBehaviour, IHarmable {

	public int enemyPlayerNumber = 0;

	private float currentHealth;
	private float maxHealth = 50;
	private Color teamColor;
	private Transform body;
	private SpriteRenderer bodySprite;
	private float maxDamageAnimationTimer = 0.1f;
	private float currentDamageAnimationTimer;
	private Color whiteColor = new Color(1, 1, 1);
	

	void Start () {
		currentDamageAnimationTimer = maxDamageAnimationTimer;
		currentHealth = maxHealth;
		transform.Find ("Sentry").GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
		body = transform.Find ("Body");
		bodySprite = body.GetComponent<SpriteRenderer>();
		teamColor = GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor;
	}
	
	void Update(){
		currentDamageAnimationTimer += Time.deltaTime;
		if(enemyPlayerNumber == 0){
			FetchEnemyPlayer();
		}
		if(currentDamageAnimationTimer < maxDamageAnimationTimer){
			bodySprite.color = whiteColor;
		}else{
			bodySprite.color = teamColor;
		}
	}
	
	public void ReceiveHit(float damage, GameObject attackerObject) {
		currentHealth -= damage;
		IAttacker attacker = attackerObject.GetComponent(typeof(IAttacker)) as IAttacker;
		attacker.RegisterSuccessfulAttack(0);
		currentDamageAnimationTimer = 0;
		if(currentHealth <= 0){
			DestroyMe ();
		}
	}
	
	public float CurrentHealthRatio(){
		return(currentHealth / maxHealth);
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

