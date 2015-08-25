using UnityEngine;
using System.Collections;

public class UnusedFormation : MonoBehaviour {
	public float width = 10f;
	public float height = 5f;
	public GameObject minionPrefab;
	public float formationSpeed = 1f;
	public bool movingRight = false;
	public float spawnDelay = 0.5f;
	
	private GameObject affinity;
//	private float xMin;
//	private float xMax;

	// Use this for initialization
	void Start () {
		affinity = GetComponent<Entity>().affinity;
		SpawnUntilFull ();
		
//		float zDistance = transform.position.z - Camera.main.transform.position.z;
//		Vector3 leftVector = new Vector3(0,0,zDistance);
//		Vector3 rightVector = new Vector3(1,0,zDistance);
//		Vector3 leftMost = Camera.main.ViewportToWorldPoint(leftVector);
//		Vector3 rightMost = Camera.main.ViewportToWorldPoint(rightVector);
		
//		xMin = leftMost.x;
//		xMax = rightMost.x;
	}
	
	void OnDrawGizmos () {
		Gizmos.DrawWireCube(
			transform.position,
			new Vector3(width, height)
		);
	}
	
	// Update is called once per frame
	void Update () {
//		if(!movingRight){
//			transform.position += Vector3.left * formationSpeed * Time.deltaTime;
//		}else if(movingRight){
//			transform.position += Vector3.right * formationSpeed * Time.deltaTime;
//		}
		
//		float newX = Mathf.Clamp (transform.position.x, xMin, xMax);
//		transform.position = new Vector3(newX, transform.position.y);
		
//		if(!movingRight && (transform.position.x - width/2) <= xMin){
//			movingRight = true;
//		}else if(movingRight && (transform.position.x + width/2) >= xMax){
//			movingRight = false;
//		}
		
		if(AllMembersDead()){
			SpawnUntilFull ();
		}
	}
	
	Transform NextFreePosition(){
		foreach(Transform childPosition in transform){
			if (childPosition.childCount <= 0){
				return childPosition;
			}
		}
		return null;
	}
	
	void SpawnUntilFull (){
		Transform freePosition = NextFreePosition();
		
		if(freePosition){
			GameObject minion = Object.Instantiate(minionPrefab, freePosition.position, Quaternion.identity) as GameObject;
			minion.transform.parent = freePosition;
			minion.GetComponent<Entity>().affinity = affinity;
			minion.GetComponent<Entity>().reversePosition = GetComponent<Entity>().reversePosition;
//			if(GetComponent<Entity>().reversePosition){
//				minion.transform.eulerAngles = new Vector3(
//				minion.transform.eulerAngles.x,
//				minion.transform.eulerAngles.y,
//				minion.transform.eulerAngles.z + 180);
//			}
		}
		
		if(NextFreePosition()){
			Invoke ("SpawnUntilFull", spawnDelay);
		}
		
	}
	
	bool AllMembersDead () {
		foreach(Transform childPosition in transform){
			if (childPosition.childCount > 0){
				return false;
			}
		}
		return true;
	}
}
