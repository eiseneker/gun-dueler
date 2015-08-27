using UnityEngine;
using System.Collections;

public class MagnetMissile : MonoBehaviour {
	public Player player;
	
	private float timeSinceLastFire;
	private float fireDelay = 3f;
	private float magnetSpeed = 7;
	private GameObject magnetPrefab;
	private float yVector = 1;
	
	
	void Start () {
		timeSinceLastFire = fireDelay;
		magnetPrefab = Resources.Load ("magnet") as GameObject;
	}
	
	void Update () {
		timeSinceLastFire += Time.deltaTime;
	}
	
	public void Fire () {
		if(timeSinceLastFire >= fireDelay){
			GameObject magnetObject = Instantiate(magnetPrefab, transform.position, Quaternion.identity) as GameObject;
			Magnet magnet = magnetObject.GetComponent<Magnet>();
			magnet.speed = magnetSpeed;
			magnet.specialWeapon = this;
			magnet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
			if(player.reversePosition) {
				magnet.transform.eulerAngles = new Vector3(
					magnet.transform.eulerAngles.x,
					magnet.transform.eulerAngles.y,
					magnet.transform.eulerAngles.z + 180);
			}
			magnet.vector = Vector3.up * yVector;
			
			timeSinceLastFire = 0f;
		}
	}
}
