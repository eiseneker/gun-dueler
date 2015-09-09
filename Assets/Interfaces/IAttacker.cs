using UnityEngine;
using System.Collections;

public interface IAttacker {

	void RegisterSuccessfulAttack(float value);
	
	void RegisterSuccessfulDestroy(float value);
	
}
