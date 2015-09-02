using UnityEngine;
using System.Collections;

public class Core : MonoBehaviour, IHarmable {

	private float currentHealth;
	private float maxHealth = 100;

	void Start () {
		currentHealth = maxHealth;
		transform.Find ("Sentry").GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
		Transform body = transform.Find ("Body");
		body.GetComponent<SpriteRenderer>().color = GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor;
	}
	
	public void ReceiveHit(float damage, GameObject attackerObject) {
		currentHealth -= damage;
		IAttacker attacker = attackerObject.GetComponent(typeof(IAttacker)) as IAttacker;
		attacker.RegisterSuccessfulAttack(5);
		print (currentHealth);
		if(currentHealth <= 0){
			Destroy (gameObject);
		}
	}
}
