using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeDistraction : EnemyDistraction {

	float distToDistraction;

	public override void Start () {
		base.Start ();
		distToDistraction = Mathf.Infinity;
		nonDistractionLayers = 1 << LayerMask.NameToLayer ("Smoke");
	}

	protected override void LateUpdate() {
		base.LateUpdate ();
		if (distracted) {
			if (!distraction) {
				distToDistraction = Mathf.Infinity;
			}
		}
	}

	protected override void CheckForDistraction () {
		Collider[] hits = Physics.OverlapSphere (transform.position, checkRadius, nonDistractionLayers, QueryTriggerInteraction.Collide);
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
		if (!manager.Sight.Alerted) {
			distraction = obj;
			distToDistraction = Vector3.Distance (transform.position, obj.transform.position);
			distracted = true;
			pathToDistraction = manager.Graph.FindShortestPath (manager.Movement.CurrVertexIndex, vertex);
			if (pathToDistraction.Count > 0) {
				manager.Movement.Path = pathToDistraction;
			}
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
