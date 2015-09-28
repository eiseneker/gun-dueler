using UnityEngine;
using System.Collections;

public class Road : MonoBehaviour, IShreddable {

	private LoadMarker loadMarker;
	
	private static ArrayList roads = new ArrayList();
	
	private float hazardChance = 0f;
	private float neutralChance = 1f;

	// Use this for initialization
	void Start () {
		loadMarker = transform.Find ("LoadMarker").GetComponent<LoadMarker>();
		loadMarker.road = this;
		roads.Add (this);
		transform.eulerAngles = new Vector3(
			32,
			transform.eulerAngles.y,
			transform.eulerAngles.z);
		foreach(Transform child in transform.Find ("Spawns")){
			float randomValue = Random.value;
		
			if(randomValue <= hazardChance){
				GameObject rock = Instantiate(Resources.Load ("Rock"), SpawnPosition (child.transform.position), Quaternion.identity) as GameObject;
				rock.transform.localScale = rock.transform.localScale * .1f;
			}else if(randomValue <= hazardChance + neutralChance){
				GameObject neutralContainer = Instantiate(Resources.Load ("Minions/Neutral Container"), SpawnPosition (child.transform.position), Quaternion.identity) as GameObject;
				Transform neutralBody = neutralContainer.transform.Find("Ship").transform;
				neutralBody.GetComponent<Entity>().neutral = true;
				OrientationHelper.RotateTransform(neutralBody.transform, -90);
			}
		}
	}
	
	
	public void LoadNewRoad(){
		GameObject road = Instantiate (gameObject, new Vector3(transform.position.x + 100, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
		road.transform.parent = transform.parent;
	}
	
	public void UnloadFirstRoad(){
		Road road = roads[0] as Road;
		if(road != this){
			roads.Remove(road);
			Destroy (road.gameObject);
		}
	}
	
	public void DestroyMe(){
		Destroy(gameObject);
	}
	
	private Vector3 SpawnPosition(Vector3 rungPosition){
		float yPosition = Random.Range (-5.5f, 5.5f);
		return(new Vector3(rungPosition.x, yPosition, rungPosition.z));
	}
}
