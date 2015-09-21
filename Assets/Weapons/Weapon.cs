using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	public Player player;
	
	protected float timeSinceLastFire;
	protected float fireDelay;
	protected int currentBulletsInPlay;
	protected int maxBulletsInPlay;
	protected float projectileSpeed;
	
	protected int currentAmmoCount;
	
	public virtual void Start() {
		currentAmmoCount = MaxAmmoCount();
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
	
	protected virtual void Update () {
		timeSinceLastFire += Time.deltaTime;
	}
	
	protected bool CanFire() {
		return(!AtMaxBullets () && HasFinishedCooldown ());
	}
	
	protected void OrientProjectile(Projectile projectile){
		if(player.reversePosition) projectile.RotateMe(180);
	}
	
	public virtual float CurrentAmmoRatio(){
		return((float)currentAmmoCount/(float)MaxAmmoCount());
	}
	
	protected virtual int MaxAmmoCount(){
		return(0);
	}
	
}
