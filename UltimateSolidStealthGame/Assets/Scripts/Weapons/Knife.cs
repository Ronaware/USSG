using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Subclass of Equipment class that allows user to use knife
*/
public class Knife : Equipment {

	/*
		distance in front of user knife hits
	*/
	[SerializeField]
	float knifeRange = 1.0f;
	/*
		amount of damage the knife does
	*/
	[SerializeField]
	float damage = 1.0f;

	void Awake () {
		base.Awake ();
		count = -1;
	}

	/*
		override of base class
		attacks in front of user, if object hit has a HealthManager component, causes damage
	*/
	public override void UseEquipment () {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, knifeRange)) {
			GameObject target = hit.collider.gameObject;
			HealthManager targetHealth = target.GetComponent<HealthManager> ();
			if (targetHealth) {
				targetHealth.Attack (damage);
			}
		}
	}
}
