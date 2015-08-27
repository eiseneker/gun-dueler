using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour, IHarmable {
	public float speed;
	public int playerNumber;
	public GameObject shieldPrefab;
	public float maxHealth;
	public bool reversePosition;
	
	public static List<GameObject> players = new List<GameObject>();
	
	private float currentHealth;
	private PlayerHitState playerHitState;
	private Vulcan vulcan;
	private Shotgun shotgun;
	private MagnetMissile magnetMissile;
	private GameObject body;
	private GameObject shieldObject;
	private Shield shield;
	
	void Start(){
		players.Add (gameObject);
		playerHitState = gameObject.AddComponent ("PlayerHitState") as PlayerHitState;
		playerHitState.player = gameObject;
		vulcan = gameObject.AddComponent ("Vulcan") as Vulcan;
		vulcan.player = this;
		shotgun = gameObject.AddComponent ("Shotgun") as Shotgun;
		shotgun.player = this;
		magnetMissile = gameObject.AddComponent ("MagnetMissile") as MagnetMissile;
		magnetMissile.player = this;
		reversePosition = GetComponent<Entity>().reversePosition;
		body = transform.Find ("Body").gameObject;
		body.GetComponent<ParticleSystem>().startColor = GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor;
		shieldObject = Instantiate (shieldPrefab, transform.position, Quaternion.identity) as GameObject;
		shieldObject.transform.parent = gameObject.transform;
		shield = shieldObject.GetComponent<Shield>();
		currentHealth = maxHealth;
	}
		
	void Update () {
		float xMovement = Input.GetAxis ("Player"+playerNumber+"_X");
		float yMovement = Input.GetAxis ("Player"+playerNumber+"_Y");
		
		float moveFactor = 1;
		
		if(reversePosition) {
			moveFactor = -1;
		}
		
		xMovement *= moveFactor;
		yMovement *= moveFactor;
		
		if((transform.position.x > -4.3 && xMovement * moveFactor < 0) || (transform.position.x < 4.3 && xMovement * moveFactor > 0)){
			transform.Translate(Vector3.right * xMovement * Time.deltaTime * speed);
		}
		
		if((transform.position.y > -4.90 && yMovement * moveFactor > 0) || (transform.position.y < 4.9 && yMovement * moveFactor < 0)){
			transform.Translate(Vector3.down * yMovement * Time.deltaTime * speed);
		}
		
		if(Input.GetAxis ("Player"+playerNumber+"_Shotgun") == 1){
			shotgun.Fire ();
		}
		
		if(Input.GetAxis ("Player"+playerNumber+"_MagnetMissile") == 1){
			magnetMissile.Fire ();
		}
		
		if(Input.GetAxis ("Player"+playerNumber+"_Shield") == 1){
			shield.ShieldUp();
		}else if(Input.GetAxis ("Player"+playerNumber+"_Vulcan") == 1){
			vulcan.Fire();
			shield.ShieldDown();
		}else{
			shield.ShieldDown();
		}
		
		if(currentHealth <= 0){
			Destroy (gameObject);
		}
	}
	
	public void ReceiveHit(float damage) {
		if(shield.IsShieldUp()){
			shield.DamageShield(20);
		}else{
			playerHitState.RegisterHit();
			currentHealth -= damage;
		}
		
	}
	
	public bool IsCritical(){
		return(playerHitState.IsCritical());
	}
	
	public float CurrentHealthRatio(){
		return(currentHealth / maxHealth);
	}
	
}
