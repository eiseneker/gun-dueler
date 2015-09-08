using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour, IHarmable, IAttacker {
	public GameObject shieldPrefab;
	public float maxHealth;
	public bool reversePosition;
	public float maxExValue = 100;
	
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
	private bool exMode = false;
	private float speed;
	private float currentExValue = 0;
	private float defaultSpeed = 5.1f;
	
	void Start(){
		speed = defaultSpeed;
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
		foreach(Transform child in body.transform){
			Exhaust exhaust = child.GetComponent<Exhaust>();
			if(exhaust){
				exhaust.SetColor(GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor);
			}
		}
		shieldObject = Instantiate (shieldPrefab, transform.position, Quaternion.identity) as GameObject;
		shieldObject.transform.parent = gameObject.transform;
		shield = shieldObject.GetComponent<Shield>();
		shield.player = this;
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
			
			if(Input.GetAxis ("Player"+playerNumber+"_Ex") == 1){
				EnterExMode();
			}else{
				ExitExMode();
			}
			
			if((xMovement != 0 || yMovement != 0) && IsInExMode () && SpendEx (1)){
				speed = defaultSpeed * 1.5f;
			}else{
				speed = defaultSpeed;
			}
			
			if((transform.position.x > -5 && xMovement * moveFactor < 0) || (transform.position.x < 5 && xMovement * moveFactor > 0)){
				transform.Translate(Vector3.right * xMovement * Time.deltaTime * speed);
			}
			
			if((transform.position.y > -6.25f && yMovement * moveFactor > 0) || (transform.position.y < 6.25f && yMovement * moveFactor < 0)){
				transform.Translate(Vector3.down * yMovement * Time.deltaTime * speed);
			}
			
			if(Input.GetAxis ("Player"+playerNumber+"_SpecialWeapon1") == 1){
				shotgun.Fire (IsInExMode());
			}else if(Input.GetAxis ("Player"+playerNumber+"_SpecialWeapon2") == 1){
				magnetMissile.Fire (IsInExMode());
			}else if(Input.GetAxis ("Player"+playerNumber+"_SuperWeapon") == 1){
				gigaBeam.Fire ();
			}else if(Input.GetAxis ("Player"+playerNumber+"_Defensive") == 1){
				shield.ShieldUp(IsInExMode());
			}else if(Input.GetAxis ("Player"+playerNumber+"_PrimaryWeapon") == 1){
				vulcan.Fire(IsInExMode());
				shield.ShieldDown();
			}else{
				shield.ShieldDown();
			}
		}else{
			shield.ShieldDown ();
		}
		
	}
	
	public void ReceiveHit(float damage, GameObject attackerObject) {
		if(shield.IsShieldUp()){
			shield.DamageShield(20);
		}else{
			playerHitState.RegisterHit();
			currentHealth -= damage;
			if(attackerObject){
				IAttacker attacker = attackerObject.GetComponent(typeof(IAttacker)) as IAttacker;
				if(attacker != null){
					attacker.RegisterSuccessfulAttack(5);
				}
			}
			
			if(currentHealth <= 0){
				if(attackerObject){
					IAttacker attacker = attackerObject.GetComponent(typeof(IAttacker)) as IAttacker;
					if(attacker != null){
						attacker.RegisterSuccessfulDestroy(25);
					}
				}
				Destroy (gameObject);
			}
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
	
	public void RegisterSuccessfulDestroy(float value){
		currentExValue = Mathf.Clamp (currentExValue + value, 0, maxExValue);
	}
	
	public bool SpendEx(float amount){
		if(currentExValue >= amount){
			currentExValue -= amount;
			return(true);
		}else{
			return(false);
		}
	}
	
	private void EnterExMode(){
		exMode = true;
	}
	
	private void ExitExMode(){
		exMode = false;
	}
	
	private bool IsInExMode(){
		return(exMode);
	}
	
}
