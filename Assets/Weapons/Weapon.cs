using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	public Player player;
	
	protected float timeSinceLastFire;
	protected float fireDelay;
	protected int currentBulletsInPlay;
	protected int maxBulletsInPlay;
	protected float projectileSpeed;
	
	public virtual void Start() {
		timeSinceLastFire = fireDelay;
	}
	
	public void RegisterBullet(){
		currentBulletsInPlay++;
	}
	
	public void UnregisterBullet(){
		currentBulletsInPlay--;
	}
	
	protected bool AtMaxBullets(){
		if(maxBulletsInPlay == 0) return(false);
		return(currentBulletsInPlay >= maxBulletsInPlay);
	}
	
	protected bool HasFinishedCooldown(){
		return(timeSinceLastFire >= fireDelay);
	}
	
	protected void Update () {
		timeSinceLastFire += Time.deltaTime;
	}
	
	protected bool CanFire() {
		return(!AtMaxBullets () && HasFinishedCooldown ());
	}
	
	protected void RotateProjectile(Projectile projectile){
		if(player.reversePosition){
			projectile.transform.eulerAngles = new Vector3(
				projectile.transform.eulerAngles.x,
				projectile.transform.eulerAngles.y,
				projectile.transform.eulerAngles.z + 180);
		}
	}
	
}
