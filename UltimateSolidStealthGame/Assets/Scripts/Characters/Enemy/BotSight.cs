using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSight : EnemySight {

	[SerializeField]
	float botPauseTime;

	int iceLayer;
	bool paused;

	protected override void Start() {
		base.Start();
		iceLayer = LayerMask.NameToLayer("IcePlayer");
	}

	protected override void Update() {
		base.Update ();
		if (pathToPlayer.Count == 2) {
			Vector3 toPlayer = (playerMovement.transform.position - transform.position).normalized;
			if (transform.forward.normalized == toPlayer) {
				//StartCoroutine ("PauseBot");
			}
		}
	}

	protected override void CheckSightline () {
		frames = (frames + 1) % numFramesToResetPath;
		if (playerMovement != null && manager.Movement != null) {
			if (frames == (numFramesToResetPath - 1)) {
				Vector3 toPlayer = playerMovement.transform.position - gameObject.transform.position;
				Vector3 front = gameObject.transform.forward;
				float angle = Vector3.Angle (front.normalized, toPlayer.normalized);
				float fov = (alerted) ? alertedFOV : FOV;
				if (angle <= fov && toPlayer.magnitude <= sightDistance) {
					RaycastHit hit;
					if (Physics.Raycast (transform.position, toPlayer, out hit, Mathf.Infinity, ignoreEnemiesLayer)) {
						if (hit.transform.CompareTag ("Player") && (hit.transform.gameObject.layer != iceLayer)) {
							pathToPlayer = manager.Graph.FindShortestPath (manager.Movement.CurrVertexIndex, playerMovement.CurrVertexIndex);
							if (pathToPlayer.Count > 0) {
								alerted = true;
								if (manager.Distraction) {
									manager.Distraction.Distracted = false;
								}
								manager.Movement.Path = pathToPlayer;
							}
						}
					}
				} 
			}
		}
	}

	IEnumerator PauseBot() {
		if (!paused) {
			paused = true;
			manager.Movement.StopAllCoroutines ();
			manager.Movement.enabled = false;
			yield return new WaitForSeconds (botPauseTime);
			manager.Movement.enabled = true;
			paused = false;
		}
	}
}
