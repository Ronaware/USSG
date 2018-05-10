using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Bullet Class
*/
public class Bullet : MonoBehaviour {

	/*
		Amount of damage bullet does
	*/
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
