using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cigarette : Equipment {

	[SerializeField]
	GameObject cigarettePackPrefab;
	[SerializeField]
	float smokeBreakLength;
	[SerializeField]
	float packLifetime = 5.0f;

	PlayerManager manager;

	void Start () {
		manager = GetComponentInParent<PlayerManager> ();
	}

	public override void UseEquipment () {
		if (manager.Movement) {
			manager.Movement.StopMoving ();
			StartCoroutine("SmokeBreak");
		}
	}

	IEnumerator SmokeBreak() {
		//start smoke animation
		if (count > 0) {
			for (int i = 0; i < smokeBreakLength; i++) {
				yield return new WaitForSeconds (0.1f);
				if (manager.Movement.Movement != Vector3.zero) {
					//end smoke animation
					StopCoroutine ("SmokeBreak");
				}
			}
			//end smoke animation
			//Drop smoke
			GameObject smokes = Instantiate (cigarettePackPrefab, transform.position, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
			Destroy (smokes, packLifetime);
			count--;
		}
	}
}
