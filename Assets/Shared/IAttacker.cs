using UnityEngine;
using System.Collections;

interface IAttacker {

	void RegisterSuccessfulAttack(float value);
	
	void RegisterSuccessfulDestroy(float value);
	
}
