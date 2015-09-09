using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUp : MonoBehaviour, IShreddable {

	public int level;
	public GameObject powerUpPositionObject;

	private float speed = 1.2f;
	private float rotationsPerSecond = 30f;
	private float maxRotationTimer;
	private float currentRotationTimer;
	private float rotationFactor;
	private Color[] colors = { Color.yellow, Color.red };
	
	public void SetPowerUpPosition(GameObject inputPosition){
		powerUpPositionObject = inputPosition;
	}
	
	public void DestroyMe(){
		PowerUpController.DecrementPowerUps();
		Destroy (gameObject);
	}
	
	void Start(){
		maxRotationTimer = Random.Range (1f, 5f);
		transform.Find("Body").GetComponent<SpriteRenderer>().color = colors[level - 1];
		List<PowerUpPosition> openPositions = PowerUpController.openPowerUpPositions;
		powerUpPositionObject = PowerUpController.openPowerUpPositions[Random.Range (0, openPositions.Count)].gameObject;
	}

	void Update(){
		transform.Translate (Vector3.up * Time.deltaTime * speed);
		if(currentRotationTimer <= 0){
			rotationFactor = Random.Range (-4f, 4f);
			maxRotationTimer = Random.Range (1f, 10f);
			currentRotationTimer = maxRotationTimer;
		}else{
			currentRotationTimer -= Time.deltaTime;
		}
	
		float probability = rotationsPerSecond * Time.deltaTime;
		if(Random.value < probability){
			transform.Rotate(0, 0, rotationFactor);
		}
	
		transform.Translate (Vector3.up * speed * Time.deltaTime);
	}

	void OnTriggerEnter2D (Collider2D collision) {
		Player player = collision.gameObject.GetComponent<Player>();
		PowerUpPosition powerUpPosition = powerUpPositionObject.GetComponent<PowerUpPosition>();
		if(player){
		 	if(!powerUpPosition.IsTaken ()){
				Fleet fleet = player.GetComponent<Entity>().affinity.GetComponent<Fleet>();
//				GameObject minionFormation = fleet.AddMinionFormation(level);
//				minionFormation.transform.position = powerUpPosition.transform.position;
				powerUpPosition.TakePosition();
			}
			DestroyMe ();
		}
	}
	
	
	
}
