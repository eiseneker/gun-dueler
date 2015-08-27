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
	public float maxHealth;
	
	private float currentHealth;
	private float timeSinceLastFire;
	private PlayerHitState playerHitState;
	private int currentBulletsInPlay = 0;
	private bool reversePosition;
	private GameObject body;
	private GameObject shieldObject;
	private Shield shield;
	
	void Start(){
		playerHitState = gameObject.AddComponent ("PlayerHitState") as PlayerHitState;
		playerHitState.player = gameObject;
		reversePosition = GetComponent<Entity>().reversePosition;
		body = transform.Find ("Body").gameObject;
		body.GetComponent<ParticleSystem>().startColor = GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor;
		shieldObject = Instantiate (shieldPrefab, transform.position, Quaternion.identity) as GameObject;
		shieldObject.transform.parent = gameObject.transform;
		shield = shieldObject.GetComponent<Shield>();
		currentHealth = maxHealth;
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
		
		if(Input.GetAxis ("Player"+playerNumber+"_Fire2") == 1){
			shield.ShieldUp();
		}else if(Input.GetAxis ("Player"+playerNumber+"_Fire1") == 1){
			Fire();
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
	
	public void RegisterBullet(){
		currentBulletsInPlay++;
	}
	
	public void UnregisterBullet(){
		currentBulletsInPlay--;
	}
	
	public bool IsCritical(){
		return(playerHitState.IsCritical());
	}
	
	public float CurrentHealthRatio(){
		return(currentHealth / maxHealth);
	}
	
	private bool AtMaxBullets(){
		return(currentBulletsInPlay >= maxBulletsInPlay);
	}
}
