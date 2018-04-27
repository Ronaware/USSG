using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	float damage;

	public float Damage {
		get { return damage; }
		set { damage = value; }
	}

	void OnCollisionEnter(Collision collision) {
		//trigger particle effect based on what you hit
		Destroy (gameObject);
	}
}
