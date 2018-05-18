using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Equipment subclass to drop CigarettePack distraction
*/
public class Cigarette : Equipment {

	/*
		prefab of CigarettePack gameobject
	*/
	[SerializeField]
	GameObject cigarettePackPrefab;
	/*
		amount of time player needs to enact smoke animation before dropping pack
	*/
	[SerializeField]
	float smokeBreakLength;
	/*
		lifetime of dropped pack
	*/
	[SerializeField]
	float packLifetime = 5.0f;

	/*
		reference to PlayerManager component
	*/
	PlayerManager manager;

	public virtual void Awake () {
		base.Awake ();
		manager = GetComponentInParent<PlayerManager> ();
	}

	/*
		Equipment baseclass override
		stops player movement and starts smoking animation coroutine
	*/
	public override void UseEquipment () {
		if (manager.Movement) {
			manager.Movement.StopMoving ();
			StartCoroutine("SmokeBreak");
		}
	}

	/*
		coroutine to start smoke animation
		drops pack if animation is not interrupted by movement
	*/
	IEnumerator SmokeBreak() {
		//start smoke animation
		if (count > 0) {
			for (int i = 0; i < smokeBreakLength; i++) {
				yield return new WaitForSeconds (0.1f);
				if (manager.Movement.Movement != Vector3.zero || (render && render.enabled == false)) {
					//end smoke animation
					StopCoroutine ("SmokeBreak");
				}
			}
			//end smoke animation
			Vector3 dropPoint = new Vector3(transform.position.x, manager.Graph.FloorTop, transform.position.z);
			GameObject smokes = Instantiate (cigarettePackPrefab, dropPoint, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
			smokes.GetComponent<CigarettePack> ().Location = manager.Movement.CurrVertexIndex;
			Destroy (smokes, packLifetime);
			count--;
		}
	}
}
