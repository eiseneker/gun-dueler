using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour, IHarmable {
	
	public virtual void ReceiveHit(float damage, GameObject attackerObject){
		
	}
	
	protected IAttacker ResolveAttacker(GameObject attackerObject){
		IAttacker attacker = null;
		if(attackerObject){
			attacker = attackerObject.GetComponent(typeof(IAttacker)) as IAttacker;
		}
		return(attacker);
	}
}
