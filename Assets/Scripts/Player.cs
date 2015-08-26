using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IHarmable {
	public float speed;
	public int playerNumber;
	public GameObject bulletPrefab;
	public int maxBulletsInPlay;
	public float bulletSpeed;
	public float fireDelay;
	public GameObject shieldPrefab;
	public float maxShieldHealth;
	public float shieldHealthPerInterval;
	public float maxBrokenShieldTime;
	
	private float timeSinceLastFire;
	private PlayerHitState playerHitState;
	private int currentBulletsInPlay = 0;
	private bool reversePosition;
	private GameObject body;
	private bool shieldIsUp = false;
	private GameObject shield;
	private float currentShieldHealth;
	private float currentBrokenShieldTime;
	
	void Start(){
		playerHitState = new PlayerHitState(gameObject);
		reversePosition = GetComponent<Entity>().reversePosition;
		body = transform.Find ("Body").gameObject;
		body.GetComponent<ParticleSystem>().startColor = GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor;
		shield = Instantiate (shieldPrefab, transform.position, Quaternion.identity) as GameObject;
		shield.transform.parent = gameObject.transform;
		shield.SetActive(false);
		currentShieldHealth = maxShieldHealth;
	}
		
	void Update () {
		timeSinceLastFire += Time.deltaTime;
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
		
		UpdateShield();
		
		if(Input.GetAxis ("Player"+playerNumber+"_Fire2") == 1){
			ShieldUp();
		}else if(Input.GetAxis ("Player"+playerNumber+"_Fire1") == 1){
			Fire();
			ShieldDown();
		}else{
			ShieldDown();
		}
		
		shield.SetActive(shieldIsUp);
		
		playerHitState.RefreshHitState();
	}
	
	public void ReceiveHit() {
		if(shieldIsUp){
			currentShieldHealth = Mathf.Clamp (currentShieldHealth - 20, 0, maxShieldHealth);
		}else{
			playerHitState.RegisterHit();
		}
		
	}
	
	private void Fire () {
		if(!AtMaxBullets() && timeSinceLastFire >= fireDelay){
			GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
			Bullet bullet = bulletObject.GetComponent<Bullet>();
			bullet.speed = bulletSpeed;
			bullet.owner = gameObject;
			bullet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
			if(reversePosition) {
				bullet.yVector = -1;
			}
			RegisterBullet ();
			timeSinceLastFire = 0f;
		}
	}
	
	private void UpdateShield() {
		if(shieldIsUp){
			if(currentShieldHealth > 0){
				currentShieldHealth = Mathf.Clamp (currentShieldHealth - shieldHealthPerInterval, 0, maxShieldHealth);
			}
		}else{
			if(currentShieldHealth < maxShieldHealth){
				currentShieldHealth = Mathf.Clamp (currentShieldHealth + shieldHealthPerInterval, 0, maxShieldHealth);
			}
		}
		if(ShieldIsBroken ()){
			currentBrokenShieldTime = Mathf.Clamp (currentBrokenShieldTime - Time.deltaTime, 0, maxBrokenShieldTime);
		}
		if(currentShieldHealth <= 0){
			BreakShield ();
		}
	}
	
	private void BreakShield() {
		currentBrokenShieldTime = maxBrokenShieldTime;
		ShieldDown ();
	}
	
	private void ShieldUp() {
		if(currentBrokenShieldTime <= 0 && !shieldIsUp){
			shieldIsUp = true;
		}
	}
	
	private bool ShieldIsBroken(){
		return(currentBrokenShieldTime > 0);
	}
	
	private void ShieldDown() {
		shieldIsUp = false;
	}
	
	public void RegisterBullet(){
		currentBulletsInPlay++;
	}
	
	public void UnregisterBullet(){
		currentBulletsInPlay--;
	}
	
	private bool AtMaxBullets(){
		return(currentBulletsInPlay >= maxBulletsInPlay);
	}
}
