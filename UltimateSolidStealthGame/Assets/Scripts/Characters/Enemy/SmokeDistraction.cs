using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Subclass of EnemyDistraction class
	Used to detect whether a cigarette distraction is in range
	If one is found, set path to it
*/
public class SmokeDistraction : EnemyDistraction {

	/*
		distance to cigarette distraction used to focus on closest distraction
	*/
	float distToDistraction;

	public override void Start () {
		base.Start ();
		distToDistraction = Mathf.Infinity;
		distractionLayers = 1 << LayerMask.NameToLayer ("Smoke");
	}

	protected override void LateUpdate() {
		base.LateUpdate ();
		if (distracted) {
			if (!distraction) {
				distToDistraction = Mathf.Infinity;
			}
		}
	}

	/*
		uses Physics.CheckSphere to find any cigarettes in range
		if found, set the closest pack as the distraction
	*/
	protected override void CheckForDistraction () {
		Collider[] hits = Physics.OverlapSphere (transform.position, checkRadius, distractionLayers, QueryTriggerInteraction.Collide);
		if (hits.Length == 0) {
			distToDistraction = Mathf.Infinity;
			return;
		}
		foreach (Collider hit in hits) {
			CigarettePack cig = hit.GetComponent<CigarettePack> ();
			if (cig) {
				if (Vector3.Distance (transform.position, hit.transform.position) < distToDistraction) {
					GameObject obj = hit.gameObject;
					SetDistraction (cig.Location, ref obj);
				}
			}
		}
	}

	public override void SetDistraction (int vertex, ref GameObject obj) {
		base.SetDistraction (vertex, ref obj);
		if (!manager.Sight.Alerted && vertex >= 0) {
			distToDistraction = Vector3.Distance (transform.position, obj.transform.position);
		}
	}

	protected override IEnumerator AtDistraction () {
		//smoking animation
		Destroy(distraction);
		yield return new WaitForSeconds (distractionTime);
		distracted = false;
		distToDistraction = Mathf.Infinity;
		enabled = true;
		manager.Movement.enabled = true;
		manager.Sight.enabled = true;
		manager.WeaponSystem.enabled = true;
		CheckForDistraction ();
	}
}
