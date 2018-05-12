using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class that allows characters to register damage
*/
public class HealthManager : MonoBehaviour {

	/*
		current health of character
	*/
	[SerializeField]
	float health;

	/*
		reference to CharacterManager component of gameObject, either Enemy or Player
	*/
	CharacterManager manager;

	public float Health {
		get { return health; }
	}
		
	void Start () {
		manager = GetComponent<CharacterManager> ();
	}

	/*
		Registers bullet damage
		@param collision - info on colliding object
	*/
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag ("Bullet") == true) {
			Bullet bullet = collision.gameObject.GetComponent<Bullet> ();
			if (bullet != null) {
				Attack (bullet.Damage);
			}
		}
	}

	/*
		causes damage to character
		@param damage - amount of damage to take
	*/
	public void Attack(float damage) {
		health = (health > 0.0f) ? health - damage : 0.0f;
		if (health <= 0.0f) {
			manager.Kill ();
		}
	}
}
