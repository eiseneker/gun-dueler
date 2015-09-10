using UnityEngine;
using System.Collections;

public class CoreSentry : MonoBehaviour, IProjectilePassable {

	private float timeSinceLastFire;
	private float fireDelay = .1f;

	void OnTriggerStay2D (Collider2D collision) {
		Entity entity = collision.gameObject.GetComponent<Entity>();
		if(entity && entity.affinity && entity.affinity != GetComponent<Entity>().affinity && entity.GetComponent(typeof(IHarmable)) != null){
			Fire (entity.gameObject);
		}
		
	}
	
	void Update () {
		timeSinceLastFire += Time.deltaTime;
	}
	
	public void Fire (GameObject target) {
		if(timeSinceLastFire >= fireDelay){
			timeSinceLastFire = 0f;
			GameObject bulletPrefab = Resources.Load ("Bullet") as GameObject;
			GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
			BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
			bullet.speed = 8f;
			bullet.owner = gameObject;
			
			Vector3 direction = target.transform.position - transform.position;
			direction.Normalize();
			bullet.vector = direction;
		}
	}
}
