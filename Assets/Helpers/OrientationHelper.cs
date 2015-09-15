using UnityEngine;
using System.Collections;

public class OrientationHelper {
	
	public static bool FacingRight(Transform transform){
		return(transform.eulerAngles.z > 225 && transform.eulerAngles.z < 315);
	}
	
	public static bool FacingDown(Transform transform){
		return(transform.eulerAngles.z > 135 && transform.eulerAngles.z < 225);
	}
	
	public static bool FacingLeft(Transform transform){
		return(transform.eulerAngles.z > 45 && transform.eulerAngles.z < 135);
	}
	
	public static bool FacingUp(Transform transform){
		return(transform.eulerAngles.z == 0 || transform.eulerAngles.z > 315 && transform.eulerAngles.z < 45);
	}
	
	public static void RotateTransform(Transform transform, float degrees){
		transform.eulerAngles = new Vector3(
			transform.eulerAngles.x,
			transform.eulerAngles.y,
			transform.eulerAngles.z + degrees);
	}
}
