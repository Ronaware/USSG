using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class that allows enemy to fire at player based on equipped weapon
*/
public class EnemyWeaponSystem : MonoBehaviour {

	/*
		prefab of weapon object
	*/
	[SerializeField]
	GameObject gunPrefab;
	/*
		time between calls of fire to wait before calling fire again
	*/
	[SerializeField]
	float fireWaitTime = 2.0f;

	/*
		whether weapon is currently being fired
	*/
	bool firing;

	/*
		reference to EnemyManager component
	*/
	EnemyManager manager;
	/*
		instance of gunPrefab
	*/
	GameObject gunInstance;
	/*
		reference to Gun component of gunInstance
	*/
	Gun gun;
	/*
		reference to enemy' MeshRenderer component
	*/
	MeshRenderer renderer;

	void Start () {
		manager = GetComponent<EnemyManager> ();
		renderer = GetComponent<MeshRenderer> ();
		if (gunPrefab) {
			gunInstance = GameObject.Instantiate (gunPrefab, transform);
			if (gunInstance != null) {
				gunInstance.layer = LayerMask.NameToLayer ("EnemyWeapon");
				gunInstance.transform.localPosition = Vector3.zero;
				gunInstance.transform.localRotation = Quaternion.identity;
				gunInstance.transform.localScale = Vector3.one;
				gun = gunInstance.GetComponent<Gun> ();
				if (gun) {
					gun.BulletsLeft = -1;
				}
			}
		}
	}

	void Update() {
		FireWeapon ();
	}

	/*
		fires weapon if specified time had passed, weapon is not currently firing, and the enemy is visible in the camera
		if player is directly in front of enemy and enemy is looking in one of the cardinal directions, fire weapon
	*/
	public void FireWeapon() {
		if (!firing && renderer.isVisible) {
			if (manager.Sight && manager.Player && manager.Movement && gun) {
				Vector3 zeroAngleVec = new Vector3 (1.0f, 0.0f, 0.0f);
				if (manager.Sight.Alerted) {
					if (Vector3.Angle (transform.forward.normalized, zeroAngleVec) % 90.0f == 0.0f) {
						RaycastHit hit;
						if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity, manager.Sight.IgnoreEnemiesLayer)) {
							if (hit.transform.CompareTag ("Player") == true) {
								firing = true;
								gun.UseEquipment ();
								StartCoroutine ("FirePause");
							}
						}
					}
				}
			}
		}
	}

	/*
		coroutine to prevent enemy from firing for specified time
	*/
	IEnumerator FirePause() {
		yield return new WaitForSeconds (fireWaitTime);
		firing = false;
		yield return null;
	}
}
