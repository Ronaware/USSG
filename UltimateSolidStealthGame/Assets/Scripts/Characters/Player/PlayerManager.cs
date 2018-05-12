using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class used to store all Player's components 
*/
public class PlayerManager : CharacterManager {

	/*
		references to player components
	*/
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

	/*
		called when player dies to turn off all components
	*/
	public override void Kill() {
		Debug.Log ("Player Dead");
		movement.enabled = false;
		weaponSystem.enabled = false;
		health.enabled = false;
		GetComponent<SwipeManager> ().enabled = false;
		GetComponent<Collider> ().enabled = false;
	}
}
