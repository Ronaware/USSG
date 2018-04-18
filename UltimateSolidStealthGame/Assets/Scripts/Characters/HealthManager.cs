using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

	[SerializeField]
	float health;

	CharacterManager manager;

	public float Health {
		get { return health; }
	}

	// Use this for initialization
	void Start () {
		manager = GetComponent<CharacterManager> ();
	}
	
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag ("Bullet") == true) {
			Bullet bullet = collision.gameObject.GetComponent<Bullet> ();
			if (bullet != null && bullet.Owner != gameObject.tag) {
				Attack (bullet.Damage);
			}
		}
	}

	public void Attack(float damage) {
		health = (health > 0.0f) ? health - damage : 0.0f;
		if (health == 0.0f) {
			manager.Kill ();
		}
	}
}
