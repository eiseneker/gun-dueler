using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour, IProjectilePassable {
	public GameObject owner;
	public Weapon weapon;
	
	protected GameObject affinity;
	protected float maxLifespan = 2;
	
	private float currentLifespan;

	public virtual void Start () {
		if(owner == null) {
			if(weapon){
				owner = weapon.player.gameObject;
			}
		}
		affinity = owner.GetComponent<Entity>().affinity;
		if(weapon == null){
			transform.Find ("Body").gameObject.layer = LayerMask.NameToLayer("Player " + (3 - affinity.GetComponent<Fleet>().playerNumber) + " Visible");
		}
	}
	
	public virtual void Update(){
		currentLifespan += Time.deltaTime;
		if(currentLifespan >= maxLifespan){
			DestroyMe ();
		}
	}
	
	public void DestroyMe(){
		if(weapon){
			weapon.UnregisterBullet();
		}
		Destroy(gameObject);
	}
	
	protected Color GetTeamColor(){
		return(affinity.GetComponent<Fleet>().teamColor);
	}
	
	protected void DetermineHit(Collider2D collision, bool destroysSelfOnHit){
		Entity hitEntity = collision.gameObject.GetComponent<Entity>();
		if(hitEntity && hitEntity.affinity != affinity){
			IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
			if(harmedObject != null){
				harmedObject.ReceiveHit(DamageValue (), owner, gameObject);
				if(destroysSelfOnHit) DestroyMe ();
			}
		}else if(hitEntity && hitEntity.gameObject != owner){
			IProjectilePassable iProjectilePassable = collision.gameObject.GetComponent(typeof(IProjectilePassable)) as IProjectilePassable;
			if(iProjectilePassable == null){
				if(DestroysSelfOnFriendlyHit()) DestroyMe ();
			}
		}
	}
	
	protected void DetermineHit(Collision2D collision, bool destroysSelfOnHit){
		Entity hitEntity = collision.gameObject.GetComponent<Entity>();
		if(hitEntity && hitEntity.affinity != affinity){
			IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
			if(harmedObject != null){
				harmedObject.ReceiveHit(DamageValue (), owner, gameObject);
				if(destroysSelfOnHit) DestroyMe ();
			}
		}else if(hitEntity && hitEntity.gameObject != owner){
			IProjectilePassable iProjectilePassable = collision.gameObject.GetComponent(typeof(IProjectilePassable)) as IProjectilePassable;
			if(iProjectilePassable == null){
				if(DestroysSelfOnFriendlyHit()) DestroyMe ();
			}
		}
	}
	
	public void RotateMe(float degrees){
		OrientationHelper.RotateTransform(transform, degrees);
	}
	
	protected virtual float DamageValue(){
		return(1);
	}
	
	protected virtual bool DestroysSelfOnFriendlyHit(){
		return(true);
	}
}
