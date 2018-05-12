using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

	GameObject player;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
		
	void LateUpdate () {
		if (player != null) {
			transform.position = player.transform.position;
		}
	}
}
