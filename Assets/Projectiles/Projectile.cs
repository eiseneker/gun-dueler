using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public GameObject owner;
	public Weapon weapon;
	
	protected GameObject affinity;

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
				harmedObject.ReceiveHit(DamageValue (), owner);
				if(destroysSelfOnHit) DestroyMe ();
			}
		}
	}
	
	public void RotateMe(float degrees){
		transform.eulerAngles = new Vector3(
			transform.eulerAngles.x,
			transform.eulerAngles.y,
			transform.eulerAngles.z + degrees);
	}
	
	protected virtual float DamageValue(){
		return(1);
	}
}
