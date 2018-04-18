using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSystem : MonoBehaviour {

	[SerializeField]
	GameObject gunPrefab;
	[SerializeField]
	float fireWaitTime = 2.0f;

	bool firing;

	EnemyManager manager;
	GameObject gunInstance;
	Gun gun;

	// Use this for initialization
	void Start () {
		manager = GetComponent<EnemyManager> ();
		if (gunPrefab) {
			gunInstance = GameObject.Instantiate (gunPrefab, transform);
			if (gunInstance != null) {
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

	public void FireWeapon() {
		if (!firing) {
			if (manager.Sight && manager.Player && manager.Movement && gun) {
				Vector3 zeroAngleVec = new Vector3 (1.0f, 0.0f, 0.0f);
				if (manager.Movement.Alerted == true) {
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

	IEnumerator FirePause() {
		yield return new WaitForSeconds (fireWaitTime);
		firing = false;
		yield return null;
	}
}
