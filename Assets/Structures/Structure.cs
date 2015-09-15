using UnityEngine;
using System.Collections;

public class Structure : Agent {

	public Truck truck;

	float Velocity(){
		return(truck.Velocity());
	}
}
