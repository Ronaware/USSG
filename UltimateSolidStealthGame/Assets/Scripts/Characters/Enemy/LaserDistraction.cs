using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDistraction : EnemyDistraction {

	public override void Start () {
		base.Start ();
		nonDistractionLayers = 1 << LayerMask.NameToLayer ("Laser");
	}

	protected override void LateUpdate() {
		base.LateUpdate ();
	}

	protected override void CheckForDistraction () {
		Collider[] hits = Physics.OverlapSphere (transform.position, checkRadius, nonDistractionLayers, QueryTriggerInteraction.Collide);
		foreach (Collider hit in hits) {
			if (!Physics.Linecast (transform.position, hit.transform.position, manager.Sight.IgnoreEnemiesLayer, QueryTriggerInteraction.Ignore)) {
				LaserTarget target = hit.GetComponent<LaserTarget> ();
				if (target) {
					GameObject obj = hit.gameObject;
					SetDistraction (target.Location, ref obj);
				}
			}
		}
	}

	public override void SetDistraction (int vertex, ref GameObject obj) {
		if (!manager.Sight.Alerted && vertex >= 0) {
			distraction = obj;
			distracted = true;
			pathToDistraction = manager.Graph.FindShortestPath (manager.Movement.CurrVertexIndex, vertex);
			if (pathToDistraction.Count > 0) {
				manager.Movement.Path = pathToDistraction;
			}
		}
	}

	protected override IEnumerator AtDistraction () {
		//laser animation
		manager.Movement.Turn(distraction.transform.forward);
		Destroy(distraction);
		yield return new WaitForSeconds (distractionTime);
		distracted = false;
		enabled = true;
		manager.Movement.enabled = true;
		manager.Sight.enabled = true;
		manager.WeaponSystem.enabled = true;
		pathToDistraction.Clear ();
	}
}
