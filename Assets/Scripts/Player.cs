using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IHarmable {
	public float speed;
	public int playerNumber;
	public GameObject bulletPrefab;
	public int maxBulletsInPlay;
	public float bulletSpeed;
	public float fireDelay;
	
	private float timeSinceLastFire;
	private PlayerHitState playerHitState;
	private int currentBulletsInPlay = 0;
	private bool reversePosition;
	private GameObject body;
	
	void Start(){
		playerHitState = new PlayerHitState(gameObject);
		reversePosition = GetComponent<Entity>().reversePosition;
		body = transform.Find ("Body").gameObject;
		body.GetComponent<ParticleSystem>().startColor = GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor;
	}
		
	void Update () {
		timeSinceLastFire += Time.deltaTime;
		float xMovement = Input.GetAxis ("Player"+playerNumber+"_X");
		float yMovement = Input.GetAxis ("Player"+playerNumber+"_Y");
		
		if(reversePosition) {
			xMovement *= -1;
			yMovement *= -1;
		}
		
		if((transform.position.x > -8.67 && xMovement < 0) || (transform.position.x < 9.02 && xMovement > 0)){
			transform.Translate(Vector3.right * xMovement * Time.deltaTime * speed);
		}
		
		if((transform.position.y > -4.90 && yMovement > 0) || (transform.position.y < 4.82 && yMovement < 0)){
			transform.Translate(Vector3.down * yMovement * Time.deltaTime * speed);
		}
		
		if(Input.GetAxis ("Player"+playerNumber+"_Fire1") == 1){
			Fire();
		}
		
		playerHitState.RefreshHitState();
	}
	
	public void ReceiveHit() {
		playerHitState.RegisterHit();
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
	
	private bool AtMaxBullets(){
		return(currentBulletsInPlay >= maxBulletsInPlay);
	}
}
