using UnityEngine;
using System.Collections;

public class Structure : Agent {

	public Truck truck;
	public static int structureCount;

	float Velocity(){
		return(truck.Velocity().x);
	}
}
