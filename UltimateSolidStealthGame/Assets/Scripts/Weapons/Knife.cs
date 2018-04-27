using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Equipment {

	[SerializeField]
	float knifeRange = 1.0f;
	[SerializeField]
	float damage = 1.0f;

	// Use this for initialization
	void Start () {
		count = -1;
	}

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
