using UnityEngine;
using System.Collections;

interface IHarmable {

	void ReceiveHit(float damage, GameObject attacker);
	
}
