using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

	[SerializeField]
	float health;

	// Use this for initialization
	void Start () {
		
	}
	
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag ("Bullet") == true) {
			Bullet bullet = collision.gameObject.GetComponent<Bullet> ();
			if (bullet != null && bullet.Owner != gameObject.tag) {
				health--;
				if (health <= 0) {
					//turn off movement (kill()), make both movements inherit from base class
				}
			}
		}
	}
}
