using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour, IHarmable, IAttacker {
	public float speed;
	public GameObject shieldPrefab;
	public float maxHealth;
	public bool reversePosition;
	public float currentExValue = 0;
	
	public static List<GameObject> players = new List<GameObject>();
	
	private float currentHealth;
	private PlayerHitState playerHitState;
	private Vulcan vulcan;
	private Shotgun shotgun;
	private MagnetMissile magnetMissile;
	private GigaBeam gigaBeam;
	private GameObject body;
	private GameObject shieldObject;
	private Shield shield;
	private bool IsInputLocked = false;
	private int playerNumber;
	private float maxExValue = 100;
	
	void Start(){
		players.Add (gameObject);
		playerHitState = gameObject.AddComponent ("PlayerHitState") as PlayerHitState;
		playerHitState.player = gameObject;
		vulcan = gameObject.AddComponent ("Vulcan") as Vulcan;
		vulcan.player = this;
		shotgun = gameObject.AddComponent ("Shotgun") as Shotgun;
		shotgun.player = this;
		gigaBeam = gameObject.AddComponent ("GigaBeam") as GigaBeam;
		gigaBeam.player = this;
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
		if(!IsInputLocked){
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
			
			if(Input.GetAxis ("Player"+playerNumber+"_SpecialWeapon1") == 1){
				shotgun.Fire ();
			}else if(Input.GetAxis ("Player"+playerNumber+"_SpecialWeapon2") == 1){
				magnetMissile.Fire ();
			}else if(Input.GetAxis ("Player"+playerNumber+"_SuperWeapon") == 1){
				if(currentExValue >= maxExValue){
					gigaBeam.Fire ();
					currentExValue -= maxExValue;
				}
			}else if(Input.GetAxis ("Player"+playerNumber+"_Defensive") == 1){
				shield.ShieldUp();
			}else if(Input.GetAxis ("Player"+playerNumber+"_PrimaryWeapon") == 1){
				vulcan.Fire();
				shield.ShieldDown();
			}else{
				shield.ShieldDown();
			}
		}else{
			shield.ShieldDown ();
		}
		
		if(currentHealth <= 0){
			Destroy (gameObject);
		}
	}
	
	public void ReceiveHit(float damage, GameObject attackerObject) {
		if(shield.IsShieldUp()){
			shield.DamageShield(20);
		}else{
			playerHitState.RegisterHit();
			currentHealth -= damage;
			IAttacker attacker = attackerObject.GetComponent(typeof(IAttacker)) as IAttacker;
			attacker.RegisterSuccessfulAttack(25);
		}
		
	}
	
	public bool IsCritical(){
		return(playerHitState.IsCritical());
	}
	
	public float CurrentHealthRatio(){
		return(currentHealth / maxHealth);
	}
	
	public float CurrentExRatio(){
		return(currentExValue / maxExValue);
	}
	
	public void LockInputs(){
		IsInputLocked = true;
	}
	
	public void UnlockInputs(){
		IsInputLocked = false;
	}
	
	public void SetPlayerNumber(int inputPlayerNumber){
		playerNumber = inputPlayerNumber;
	}
	
	public void RegisterSuccessfulAttack(float value){
		currentExValue = Mathf.Clamp (currentExValue + value, 0, maxExValue);
	}
	
}
