using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	string owner;
	float damage;

	public string Owner {
		get { return owner; }
		set { owner = value; }
	}
	public float Damage {
		get { return damage; }
		set { damage = value; }
	}

	void OnCollisionEnter(Collision collision) {
		//trigger particle effect based on what you hit
		Destroy (gameObject);
	}
}
