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
		return(transform.eulerAngles.z > 315 || transform.eulerAngles.z < 45);
	}
	
	public static void RotateTransform(Transform transform, float degrees){
		transform.rotation = Quaternion.Euler(0f, 0f, degrees);
	}
	
	public static void RotateTransform(Transform transform, float degrees, float increment){
		Quaternion q = Quaternion.AngleAxis(degrees, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, q, increment * Time.deltaTime);
	}
	
	public static void FaceObject(Transform targetTransform, Transform myTransform){
		Vector3 distance = targetTransform.position - myTransform.position;
		distance.Normalize();
		
		float zRotation = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
		myTransform.rotation = Quaternion.Euler(0f, 0f, zRotation - 90);
	}
	
}
