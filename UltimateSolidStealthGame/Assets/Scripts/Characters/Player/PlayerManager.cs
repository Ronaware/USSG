using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager {

	PlayerMovement movement;
	PlayerWeaponSystem weaponSystem;
	PlayerUI ui;
	SwipeManager swipe;

	public PlayerMovement Movement {
		get { return movement; }
	}
	public PlayerWeaponSystem WeaponSystem {
		get { return weaponSystem; }
	}
	public PlayerUI Ui {
		get { return ui; }
	}

	protected override void Awake () {
		base.Awake ();
		movement = GetComponent<PlayerMovement> ();
		weaponSystem = GetComponent<PlayerWeaponSystem> ();
		swipe = GetComponent<SwipeManager> ();
		GameObject tempUI = GameObject.FindGameObjectWithTag ("UI");
		if (tempUI) {
			ui = tempUI.GetComponent<PlayerUI> ();
		}
	}
	
	public override void Kill() {
		Debug.Log ("Player Dead");
		movement.enabled = false;
		weaponSystem.enabled = false;
		health.enabled = false;
		GetComponent<SwipeManager> ().enabled = false;
		GetComponent<Collider> ().enabled = false;
	}
}
