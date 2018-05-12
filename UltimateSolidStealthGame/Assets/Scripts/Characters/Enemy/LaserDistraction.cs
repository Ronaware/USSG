using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Subclass of EnemyDistraction class
	Used to detect whether a laser pointer distraction is in range
	If one is found, set path to it
*/
public class LaserDistraction : EnemyDistraction {

	public override void Start () {
		base.Start ();
		distractionLayers = 1 << LayerMask.NameToLayer ("Laser");
	}

	protected override void LateUpdate() {
		base.LateUpdate ();
	}

	/*
	 	Uses Physics.Checksphere to detect any laser pointer in range
	 	if one is found, check whether there is direct line of sight to distraction
	 	if it is in the line of sight, set path to laser
	*/
	protected override void CheckForDistraction () {
		Collider[] hits = Physics.OverlapSphere (transform.position, checkRadius, distractionLayers, QueryTriggerInteraction.Collide);
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
		base.SetDistraction (vertex, ref obj);
	}

	protected override IEnumerator AtDistraction () {
		manager.Movement.Turn(distraction.transform.forward);
		//laser animation
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
