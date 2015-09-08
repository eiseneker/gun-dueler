using UnityEngine;
using System.Collections;

public class Formation : MonoBehaviour {
	public float width = 10f;
	public float height = 10f;
	public GameObject minionParentPrefab;
	public float maxFormationSpawnDelay;
	public float minionSpawnDelay;
	public float respawnsRemaining;
	
	private GameObject affinity;
	private int maxMinionCount;
	private int minionCount;
	private bool respawnOnNextUpdate = true;
	private float currentSpawnSetDelay;

	void Start () {
		affinity = GetComponent<Entity>().affinity;
		maxMinionCount = transform.childCount;
		QueueSpawnUntilFull (maxMinionCount);
	}
	
	void Update () {
		print (minionCount);
		currentSpawnSetDelay += Time.deltaTime;
		if(CanRespawn ()){
			respawnsRemaining -= 1;
			QueueSpawnUntilFull (maxMinionCount - minionCount);
			respawnOnNextUpdate = false;
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
	
	IEnumerator SpawnUntilFull (int remainingCallCount){
		yield return new WaitForSeconds(minionSpawnDelay);
		if(remainingCallCount > 0){
			Transform freePosition = NextFreePosition();
			if(freePosition) InitializeMinion(freePosition);
			if(NextFreePosition()){
				QueueSpawnUntilFull (remainingCallCount - 1);
			}
		}
	}
	
	public void MinionDestroyed() {
		minionCount -= 1;
		if(minionCount <= 0 && respawnsRemaining <= 0){
			print ("goooo bye-bye!");
			Destroy (gameObject);
		}else if(currentSpawnSetDelay >= maxFormationSpawnDelay){
			currentSpawnSetDelay = 0f;
			respawnOnNextUpdate = true;
		}
	}
	
	private void DestroyMe(){
		Destroy (gameObject);
	}
	
	private void QueueSpawnUntilFull(int count){
		StartCoroutine (SpawnUntilFull (count));
	}
	
	private void InitializeMinion(Transform position){
		GameObject minionParent = Object.Instantiate(minionParentPrefab, position.position, Quaternion.Inverse (transform.rotation)) as GameObject;
		minionParent.transform.parent = position;
		GameObject minion = minionParent.transform.Find ("Ship").gameObject;
		minion.GetComponent<Entity>().affinity = affinity;
		minion.GetComponent<Entity>().reversePosition = GetComponent<Entity>().reversePosition;
		minion.GetComponent<Minion>().formation = gameObject;
		print ("incrementing minion count!");
		minionCount += 1;
	}
	
	private bool CanRespawn(){
		bool atMaxMinions = minionCount >= maxMinionCount;
		bool atMaxSpawnDelay = currentSpawnSetDelay >= maxFormationSpawnDelay;
		bool hasRespawnsRemaining = respawnsRemaining > 0;
		return(respawnOnNextUpdate && !atMaxMinions && atMaxSpawnDelay && hasRespawnsRemaining);
	}	 
	
	//development only
	
	private void OnDrawGizmos () {
		Gizmos.DrawWireCube(
			transform.position,
			new Vector3(width, height)
			);
	}
}
