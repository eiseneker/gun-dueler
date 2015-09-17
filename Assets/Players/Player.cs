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
	private VehicleControls vehicleControls;
	private Truck truck;
	private float reverseIndex = 1;
	private float currentDangerTimer;
	private float maxDangerTimer = 3;
	private Truck firstTruck;
	private Truck lastTruck;
	
	void Start(){
		myRigidbody = GetComponent<Rigidbody2D>();
		players.Add (gameObject);
		playerHitState = gameObject.AddComponent<PlayerHitState>() as PlayerHitState;
		playerHitState.player = gameObject;
		vulcan = gameObject.AddComponent<Vulcan>() as Vulcan;
		vulcan.player = this;
		chaingun = gameObject.AddComponent<Chaingun>() as Chaingun;
		chaingun.player = this;
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
	}
		
	void Update () {
		ShredderContainer.ReportPosition(transform.position.x);
	
		if(enemyPlayerNumber == 0){
			FetchEnemyPlayer();
		}
		
//		ResolveDangerTime();
		UpdateTruckOrder ();
		ResolveBoundaryConditions();
	
		if(!IsInputLocked){
			float xMovement = Input.GetAxis ("Player"+playerNumber+"_X");
			float yMovement = Input.GetAxis ("Player"+playerNumber+"_Y");
			
			ManageExInput();
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
	}
	
	void ManageExInput(){
		if(Input.GetAxis ("Player"+playerNumber+"_Ex") == 1 && currentExValue >= 50){
			EnterExMode();
		}
	}
	
	void ManageActionInputs(){
		if(Input.GetAxis ("Player"+playerNumber+"_SpecialWeapon1") == 1){
			shotgun.Fire (IsInExMode());
		}else if(Input.GetAxis ("Player"+playerNumber+"_SpecialWeapon2") == 1){
			magnetMissile.Fire (IsInExMode());
		}else if(Input.GetAxis ("Player"+playerNumber+"_SuperWeapon") == 1){
			gigaBeam.Fire ();
		}else if(Input.GetAxis ("Player"+playerNumber+"_Defensive") == 1){
			shield.ShieldUp(IsInExMode());
		}else if(Input.GetAxis ("Player"+playerNumber+"_PrimaryWeapon") == 1){
			chaingun.Fire(IsInExMode());
			shield.ShieldDown();
		}else{
			shield.ShieldDown();
		}
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
}
