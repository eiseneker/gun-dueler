using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	public GameObject player;
	
	public int playerNumber;
	private Vector3  currentVelocity = new Vector3(0, 0, 0);
	private Camera camera;
	
	void Start(){
		camera = GetComponent<Camera>();
	}

	void LateUpdate () {
		if(player){
			Vector3 destination = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref currentVelocity, 0.5f);
		}else{
			player = GetPlayer();
		}
//		MovePlayerHud();
	}
	
	private GameObject GetPlayer(){
		return(GameObject.Find ("Game Root/Players/Player " + playerNumber + " Fleet/Player(Clone)"));
	}
	
	
//	void MovePlayerHud () {
//		if(player == null){
//			Destroy(gameObject);
//		}else{
//			Vector3 newPosition = new Vector3(player.transform.position.x - .25f, player.transform.position.y - 0.75f, player.transform.position.z);
//			transform.position = camera.WorldToScreenPoint(newPosition);
//		}
//	}
	
	// EXAMPLE WITH CAMERA UPSIDEDOWN
//	void OnPreCull () {
//		if(playerNumber == 2){
//			camera.ResetWorldToCameraMatrix ();
//			camera.ResetProjectionMatrix ();
//			camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(new Vector3 (1, -1, 1));
//		}
//	}
//	
//	void OnPreRender () {
//		if(playerNumber == 2){
//			GL.SetRevertBackfacing (true);
//		}
//	}
//	
//	void OnPostRender () {
//		if(playerNumber == 2){
//			GL.SetRevertBackfacing (false);
//		}
//	}


//	EXAMPLE WITH CAMERA UPSIDEDOWN
//	void OnPreCull () {
//		if(playerNumber == 2){
//			camera.ResetWorldToCameraMatrix ();
//			camera.ResetProjectionMatrix ();
//			camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(new Vector3 (1, -1, 1));
//			
//			foreach(GameObject player in Player.players){
//				player.GetComponent<Player>().SayCheese();
//			}
//		}
//	}
//	
//	void OnPreRender () {
//		if(playerNumber == 2){
////			GL.SetRevertBackfacing (true);
//			GL.SetRevertBackfacing (true);
//
//		}
//	}
////	
//	void OnPostRender () {
////		print ("postrender");
//		if(playerNumber == 2){
//			foreach(GameObject player in Player.players){
//				player.GetComponent<Player>().ResetCheese();
//			}
//			GL.SetRevertBackfacing (false);
//
//		}
//	}
//	
}
