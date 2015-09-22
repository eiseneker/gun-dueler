using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Agent, IAttacker {
	public GameObject shieldPrefab;
	public bool reversePosition;
	public float maxExValue = 100;
	public PlayerHitState playerHitState;
	public DamageBehavior damageBehavior;
	public int enemyPlayerNumber = 0;
	
	public static List<GameObject> players = new List<GameObject>();
	
	private Vulcan vulcan;
	private BombLauncher bombLauncher;
	private Chaingun chaingun;
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
	private float currentExValue = 0;
	private float currentJustRespawned;
	private Rigidbody2D myRigidbody;
	public VehicleControls vehicleControls;
	private Truck truck;
	private float reverseIndex = 1;
	private float currentDangerTimer;
	private float maxDangerTimer = 3;
	private Truck firstTruck;
	private Truck lastTruck;
	private float z = 1;
	private GameObject healthMeter;
	private float currentStunDuration = 0;
	private float maxStunDuration = 1;
	
	void Start(){
		currentStunDuration = maxStunDuration;
		myRigidbody = GetComponent<Rigidbody2D>();
		players.Add (gameObject);
		playerHitState = gameObject.AddComponent<PlayerHitState>() as PlayerHitState;
		playerHitState.player = gameObject;
		vulcan = gameObject.AddComponent<Vulcan>() as Vulcan;
		vulcan.player = this;
		GameObject chaingunObject = Instantiate (Resources.Load ("Chaingun"), transform.position, Quaternion.identity) as GameObject;
		chaingunObject.transform.parent = transform;
		chaingun = chaingunObject.GetComponent<Chaingun>() as Chaingun;
		chaingun.player = this;
		GameObject bombLauncherObject = Instantiate (Resources.Load ("Bomb Launcher"), transform.position, Quaternion.identity) as GameObject;
		bombLauncherObject.transform.parent = transform;
		bombLauncher = bombLauncherObject.GetComponent<BombLauncher>() as BombLauncher;
		bombLauncher.player = this;
		shotgun = gameObject.AddComponent<Shotgun>() as Shotgun;
		shotgun.player = this;
		gigaBeam = gameObject.AddComponent<GigaBeam>() as GigaBeam;
		gigaBeam.player = this;
		magnetMissile = gameObject.AddComponent<MagnetMissile>() as MagnetMissile;
		magnetMissile.player = this;
		reversePosition = GetComponent<Entity>().affinity.GetComponent<Fleet>().reversePosition;
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
		vehicleControls = GetComponent<VehicleControls>();
		truck = GetComponent<Entity>().affinity.GetComponent<Fleet>().truck;
		if(reversePosition) reverseIndex *= -1;
		transform.position = new Vector3(truck.transform.position.x + 6, truck.transform.position.y + (3 * reverseIndex));
		if(healthMeter == null){
			healthMeter = Instantiate ( Resources.Load ("HUD/Health Meter"), transform.position, Quaternion.identity) as GameObject;
			healthMeter.GetComponent<HealthMeter>().player = this;
		}	
	}
		
	void Update () {
		currentStunDuration += Time.deltaTime;
		ShredderContainer.ReportPosition(transform.position.x);
		
		if(currentStunDuration < maxStunDuration){
			LockInputs();
		}else{
			UnlockInputs();
		}
		
	
		if(enemyPlayerNumber == 0){
			FetchEnemyPlayer();
		}
		
//		ResolveDangerTime();
		UpdateTruckOrder ();
		ResolveBoundaryConditions();
	
		if(!IsInputLocked){
			float xMovement = Input.GetAxis ("Player"+playerNumber+"_X");
			float yMovement = Input.GetAxis ("Player"+playerNumber+"_Y") * -1;
			
			ManageVehicleControls(xMovement, yMovement);
			ManageActionInputs();
		}else{
			shield.ShieldDown ();
		}
		
		if(IsInExMode()){
			if(!SpendEx(7.5f * Time.deltaTime)){
				ExitExMode();
			}
		}
		if(vehicleControls.IsCharging()){
			body.GetComponent<SpriteRenderer>().color = Color.red;
		}else{
			body.GetComponent<SpriteRenderer>().color = Color.white;
		}
		
	}
	
	void ManageActionInputs(){
		if(Input.GetAxis ("Player"+playerNumber+"_Ex") == 1){
			bombLauncher.Fire ();
		}else if(Input.GetAxis ("Player"+playerNumber+"_SpecialWeapon1") == 1){
			chaingun.Fire(IsInExMode(), 180);
		}else if(Input.GetAxis ("Player"+playerNumber+"_SpecialWeapon2") == 1){
			chaingun.Fire(IsInExMode(), 0);
		}else if(Input.GetAxis ("Player"+playerNumber+"_SuperWeapon") == 1){
			chaingun.Fire(IsInExMode(), 270);
		}else if(Input.GetAxis ("Player"+playerNumber+"_Defensive") == 1){
			Charge ();
			chaingun.Release();
		}else if(Input.GetAxis ("Player"+playerNumber+"_PrimaryWeapon") == 1){
			chaingun.Fire(IsInExMode(), 90);
		}else{
			chaingun.Release();
		}
	}
	
	
	public override void ReceiveHit(float damage, GameObject attackerObject, GameObject attack) {
		IAttacker attacker = ResolveAttacker(attackerObject);
		bool deferHit = false;
		
		deferHit = HandledByShield();
		if(!deferHit) deferHit = HandledByCharging(attack);
		
		if(!deferHit){
			Player attackingPlayer = attack.GetComponent<Player>();
			if(attackingPlayer && attackingPlayer.vehicleControls.IsCharging ()){
				if(currentStunDuration < maxStunDuration){
					damage = 0;
				}else{
					Stun();
				}
			}
			if(!playerHitState.isHit){
				if(attacker != null) attacker.RegisterSuccessfulAttack(5);
			}
			
			playerHitState.RegisterHit();
			if(playerHitState.IsCritical()){
				damageBehavior.ReceiveDamage(damage*2);
			}else{
				damageBehavior.ReceiveDamage(damage);
			}
			
			if(damageBehavior.CurrentHealthRatio() <= 0){
				DestroyMe();
				if(attacker != null) attacker.RegisterSuccessfulDestroy(15);
			}
			
		}
		
	}
	
	private bool HandledByCharging(GameObject attack){
		bool handledByCharging = vehicleControls.IsCharging() && (attack.GetComponent<Projectile>() || attack.GetComponent<Minion>());
		if(handledByCharging){
			currentExValue += 1;
		}
		return(handledByCharging);
	}
	
	private bool HandledByShield(){
		bool shieldUp = shield.IsShieldUp();
		if(shieldUp){
			shield.DamageShield(20);
			currentExValue += 4;
		}
		return(shieldUp);
	}
	
	private void OnCollisionEnter2D(Collision2D collision){
		IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
		float damage = 1;
		if(harmedObject != null){
			if(vehicleControls.IsCharging()){
				damage = 5;
			}
			harmedObject.ReceiveHit(damage, gameObject, gameObject);
		}
	}
	
	private void Charge(){
		vehicleControls.Charge();
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
	
	private GameObject GetTruck(int index){
		return(Truck.trucks[index] as GameObject);
	}
	
	private void ResetDangerTimer(){
		currentDangerTimer = 0;
	}
	
	private void IncrementDangerTimer(){
		currentDangerTimer += Time.deltaTime;
	}
	
	private void FetchEnemyPlayer(){
		GameObject enemyPlayer = GetComponent<Entity>().EnemyPlayer();
		if(enemyPlayer){
			enemyPlayerNumber = enemyPlayer.GetComponent<Entity>().affinity.GetComponent<Fleet>().playerNumber;
		}
	}
	
	private void UpdateTruckOrder(){
		GameObject truck0 = GetTruck (0);
		GameObject truck1 = GetTruck (1);
		
		if(truck0.transform.position.x > truck1.transform.position.x){
			firstTruck = truck0.GetComponent<Truck>();
			lastTruck = truck1.GetComponent<Truck>();
		}else{
			lastTruck = truck0.GetComponent<Truck>();
			firstTruck = truck1.GetComponent<Truck>();
		}
	}
	
	private bool AheadOfFirstTruck(){
		return(transform.position.x >= firstTruck.headElement.transform.position.x + 3);
	}
	
	private void ResolveDangerTime(){
		if(currentDangerTimer >= maxDangerTimer){
			StateController.lastWinner = enemyPlayerNumber;
			GameController.LoadWinScreen();
		}
	}
	
	private void ResolveBoundaryConditions(){
		if(AheadOfFirstTruck()){
			myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, firstTruck.GetComponent<Rigidbody2D>().velocity.magnitude);
			ResetDangerTimer();
		}else if(transform.position.x <= lastTruck.lastElement.transform.position.x - 3){
			vehicleControls.Accelerate();
			IncrementDangerTimer();
		}else{
			ResetDangerTimer();
		}
	}
	
	private void ManageVehicleControls(float xMovement, float yMovement){
		if((xMovement != 0 || yMovement != 0) && IsInExMode ()){
			vehicleControls.speedMultiplier = 1.5f;
		}else{
			vehicleControls.speedMultiplier = 1;
		}
		
		if(xMovement < 0){
			vehicleControls.Brake ();
		}
		if(yMovement != 0){
			vehicleControls.Steer(yMovement);
		}else{
			vehicleControls.Straight();
			
			if(xMovement > 0){
				vehicleControls.Accelerate();
			}else{
				vehicleControls.Idle ();
			}
		}
	}
	
	private void Stun(){
		currentStunDuration = 0;
	}
}
