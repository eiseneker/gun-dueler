using UnityEngine;
using System.Collections;

public class GigaBeam : MonoBehaviour {
	public Player player;
	
	private float timeSinceLastFire;
	private float fireDelay = 3f;
	private float laserSpeed = 7;
	private GameObject laserPrefab;
	private float yVector = 1;
	
	
	void Start () {
		timeSinceLastFire = fireDelay;
		laserPrefab = Resources.Load ("GigaLaserContainer") as GameObject;
		
	}
	
	void Update () {
		timeSinceLastFire += Time.deltaTime;
	}
	
	public void Fire () {
		if(timeSinceLastFire >= fireDelay){
			print ("fire!");
			GameObject laserObject = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
			GigaLaser laser = laserObject.transform.Find ("GigaLaser").GetComponent<GigaLaser>();
//			laser.speed = laserSpeed;
			laser.specialWeapon = this;
			laser.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
			if(player.reversePosition) {
				print ("reversing!!!");
				laser.transform.eulerAngles = new Vector3(
					laser.transform.eulerAngles.x,
					laser.transform.eulerAngles.y,
					laser.transform.eulerAngles.z + 180);
			}
//			laser.vector = Vector3.up * yVector;
			
			timeSinceLastFire = 0f;
		}
	}
}
