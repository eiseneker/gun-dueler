using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	public int level;

	private float speed = 0.5f;
	private float rotationsPerSecond = 30f;
	private float maxRotationTimer;
	private float currentRotationTimer;
	private float rotationFactor;
	private Color[] colors = { Color.yellow, Color.red };
	
	void Start(){
		print (level);
		maxRotationTimer = Random.Range (1f, 5f);
		transform.Find("Body").GetComponent<SpriteRenderer>().color = colors[level - 1];
	}

	void Update(){
		transform.Translate (Vector3.up * Time.deltaTime * speed);
		if(currentRotationTimer <= 0){
			rotationFactor = Random.Range (-4f, 4f);
			maxRotationTimer = Random.Range (1f, 5f);
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
		
		if(player){
			Fleet fleet = player.GetComponent<Entity>().affinity.GetComponent<Fleet>();
			fleet.AddMinionFormation(level);
			Destroy (gameObject);
			PowerUpController.DecrementPowerUps();
		}
		
	}
	
	
	
}
