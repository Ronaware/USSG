using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelZone : MonoBehaviour {

	LevelManager lManager;

	// Use this for initialization
	void Start () {
		lManager = transform.parent.GetComponent<LevelManager> ();
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Player")) {
			lManager.FinishLevel ();
		}
	}
}
