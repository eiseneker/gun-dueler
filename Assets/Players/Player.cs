using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Agent, IAttacker {
	public GameObject shieldPrefab;
	public bool reversePosition;
	public float maxExValue = 100;
	public PlayerHitState playerHitState;
	public DamageBehavior damageBehavior;
	
	public static List<GameObject> players = new List<GameObject>();
	
	private Vulcan vulcan;
	private Shotgun shotgun;
	private MagnetMissile magnetMissile;
	private GigaBeam gigaBeam;
	private GameObject body;
	private GameObject shieldObject;
	private Shield shield;
	private bool IsInputLocked = false;
	private bool isExGainLocked = false;
	private int playerNumber;
	private bool exMode = false;
	private float speed;
	private float currentExValue = 0;
	private float defaultSpeed = 5.1f;
	private float currentJustRespawned;
	private float maxJustRespawned = 0.25f;
	private Rigidbody2D myRigidbody;
	private float accelerationFactor = 50;
	private float brakeFactor = 50;
	private float maxVelocity = 10;
	private float minVelocity = 3;
	private float idleVelocity = 5;
	private float idleFactor = 25;
	
	void Start(){
		myRigidbody = GetComponent<Rigidbody2D>();
		speed = defaultSpeed;
		players.Add (gameObject);
		playerHitState = gameObject.AddComponent<PlayerHitState>() as PlayerHitState;
		playerHitState.player = gameObject;
		vulcan = gameObject.AddComponent<Vulcan>() as Vulcan;
		vulcan.player = this;
		shotgun = gameObject.AddComponent<Shotgun>() as Shotgun;
		shotgun.player = this;
		gigaBeam = gameObject.AddComponent<GigaBeam>() as GigaBeam;
		gigaBeam.player = this;
		magnetMissile = gameObject.AddComponent<MagnetMissile>() as MagnetMissile;
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
		playerHitState.SwitchToInvincible();
		damageBehavior = GetComponent<DamageBehavior>();
		currentExValue = GetComponent<Entity>().affinity.GetComponent<Fleet>().lastExValue;
		gameObject.transform.eulerAngles = new Vector3(
			gameObject.transform.eulerAngles.x,
			gameObject.transform.eulerAngles.y,
			gameObject.transform.eulerAngles.z - 90);
	}
		
	void Update () {
		Truck leadTruck = (Truck.trucks[0] as GameObject).GetComponent<Truck>();
		float leadingY = 0;
		
		foreach(GameObject truck in Truck.trucks){
			if(truck.transform.position.x > leadingY){
				leadingY = truck.transform.position.y;
				leadTruck = truck.GetComponent<Truck>();
			}
		}
	
		if(currentJustRespawned >= maxJustRespawned){
			if(!IsInputLocked){
				float xMovement = Input.GetAxis ("Player"+playerNumber+"_X");
				float yMovement = Input.GetAxis ("Player"+playerNumber+"_Y");
				float moveFactor = 1;
				
				xMovement *= moveFactor;
				yMovement *= moveFactor;
				
				if(Input.GetAxis ("Player"+playerNumber+"_Ex") == 1 && currentExValue >= 50){
					EnterExMode();
				}
				
				if((xMovement != 0 || yMovement != 0) && IsInExMode ()){
					speed = defaultSpeed * 1.5f;
				}else{
					speed = defaultSpeed;
				}
						
				if(transform.position.x >= leadTruck.headElement.transform.position.x + 3){
					myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, leadTruck.GetComponent<Rigidbody2D>().velocity.magnitude);
				}else{
					if(xMovement == 0){
						Idle ();
					}
					
					if(xMovement > 0){
						Accelerate();
					}
					
					if(xMovement < 0){
						Brake ();
					}
				}
				if(yMovement != 0){
					Steer(yMovement);				
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
		}else{
//			transform.Translate (Vector3.up * Time.deltaTime * speed);
			currentJustRespawned += Time.deltaTime;
		}
		
		if(IsInExMode()){
			if(!SpendEx(7.5f * Time.deltaTime)){
				ExitExMode();
			}
		}
	}
	
	
	
	private void Steer(float movement){
		float velocityRange = maxVelocity - minVelocity;
		float adjustedVelocity = myRigidbody.velocity.magnitude - minVelocity;
		float velocityRatio = adjustedVelocity/velocityRange;
		transform.Translate(Vector3.left * movement * velocityRatio * Time.deltaTime * speed);
	}
	
	private void Idle(){
		if(myRigidbody.velocity.magnitude > idleVelocity){
			myRigidbody.AddRelativeForce (Vector3.up * -accelerationFactor * Time.deltaTime * speed);
		}else{
			myRigidbody.AddRelativeForce (Vector3.up * idleFactor * Time.deltaTime * speed);
			myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, idleVelocity);
		}
	}
	
	private void Brake(){
		if(myRigidbody.velocity.magnitude < minVelocity){
			myRigidbody.AddRelativeForce (Vector3.up * accelerationFactor * Time.deltaTime * speed);
			myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, minVelocity);
		}else{
			myRigidbody.AddRelativeForce (Vector3.up * brakeFactor * -1 * Time.deltaTime * speed);
		}
	}
	
	private void Accelerate(){
		myRigidbody.AddRelativeForce (Vector3.up * accelerationFactor * Time.deltaTime * speed);
		myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, maxVelocity);
	}
	
	public override void ReceiveHit(float damage, GameObject attackerObject) {
		IAttacker attacker = ResolveAttacker(attackerObject);
		
		if(shield.IsShieldUp()){
			shield.DamageShield(20);
			currentExValue += 4;
		}else{
			if(!playerHitState.isHit){
				if(attacker != null) attacker.RegisterSuccessfulAttack(5);
			}
			
			playerHitState.RegisterHit();
			damageBehavior.ReceiveDamage(damage);
			
			
			if(damageBehavior.CurrentHealthRatio() <= 0 || playerHitState.IsCritical ()){
				DestroyMe();
				if(attacker != null) attacker.RegisterSuccessfulDestroy(15);
			}
		}
		
	}
	
	public bool IsCritical(){
		return(playerHitState.IsCritical());
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
	
	public void LockExGain(){
		isExGainLocked = true;
	}
	
	public void UnlockExGain(){
		isExGainLocked = false;
	}
	
	public void SetPlayerNumber(int inputPlayerNumber){
		playerNumber = inputPlayerNumber;
	}
	
	public void RegisterSuccessfulAttack(float value){
		AdjustEx(value);
	}
	
	public void RegisterSuccessfulDestroy(float value){
		AdjustEx(value);
	}
	
	public bool SpendEx(float amount){
		if(currentExValue >= amount){
			currentExValue -= amount;
			return(true);
		}else{
			return(false);
		}
	}
	
	public bool FacingRight(){
		return(transform.eulerAngles.z > 225 && transform.eulerAngles.z < 315);
	}
	
	public bool FacingDown(){
		return(transform.eulerAngles.z > 135 && transform.eulerAngles.z < 225);
	}
	
	public bool FacingLeft(){
		return(transform.eulerAngles.z > 45 && transform.eulerAngles.z < 135);
	}
	
	public bool FacingUp(){
		return(transform.eulerAngles.z == 0 || transform.eulerAngles.z > 315 && transform.eulerAngles.z < 45);
	}
	
	public void DestroyMe(){
		if(IsInExMode()){
			ExitExMode();
			currentExValue = 0;
		}
		currentExValue /= 2;
		GetComponent<Entity>().affinity.GetComponent<Fleet>().lastExValue = currentExValue;
		Instantiate ( Resources.Load ("Explosion"), transform.position, Quaternion.identity);
		players.Remove (gameObject);
		Destroy (gameObject);
	}
	
	private void AdjustEx(float value){
		if(isExGainLocked && value > 0){
			value = 0;
		}
		currentExValue = Mathf.Clamp (currentExValue + value, 0, maxExValue);
	}
	
	private void EnterExMode(){
		LockExGain();
		body.transform.Find ("ExBody").gameObject.SetActive (true);
		exMode = true;
	}
	
	private void ExitExMode(){
		UnlockExGain();
		body.transform.Find ("ExBody").gameObject.SetActive (false);
		exMode = false;
	}
	
	public bool IsInExMode(){
		return(exMode);
	}
	
}
