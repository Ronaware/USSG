using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharacterManager {

	EnemyMovement movement;
	EnemySight sight;
	EnemyWeaponSystem weaponSystem;
	EnemyDistraction distraction;
	GameObject player;

	public EnemyMovement Movement {
		get { return movement; }
	}
	public EnemySight Sight {
		get { return sight; }
	}
	public EnemyWeaponSystem WeaponSystem {
		get { return weaponSystem; }
	}
	public GameObject Player {
		get { return player; }
	}
	public EnemyDistraction Distraction {
		get { return distraction; }

	}

	protected override void Awake () {
		base.Awake ();
		movement = GetComponent<EnemyMovement> ();
		sight = GetComponent<EnemySight> ();
		weaponSystem = GetComponent<EnemyWeaponSystem> ();
		distraction = GetComponent<EnemyDistraction>();
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	public override void Kill() {
		Debug.Log ("Enemy Dead");
		graph.vertices[movement.CurrVertexIndex].occupied = false;
		graph.vertices[movement.CurrVertexIndex].occupiedBy = "";
		graph.vertices[movement.LastVertexIndex].occupied = false;
		graph.vertices[movement.LastVertexIndex].occupiedBy = "";
		movement.enabled = false;
		sight.enabled = false;
		weaponSystem.enabled = false;
		health.enabled = false;
		distraction.enabled = false;
		movement.Nav.enabled = false;
		GetComponent<Collider> ().enabled = false;
	}
}
