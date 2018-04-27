using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : Equipment {

	[SerializeField]
	GameObject bullet;
	[SerializeField]
	int roundsPerFire = 1;
	[SerializeField]
	int bulletSpeed = 20;
	[SerializeField]
	float barrelDist= 0.25f;
	[SerializeField]
	float damage = 1.0f;
	[SerializeField]
	float timeBetweenRounds = 0.125f;
	[SerializeField]
	int bulletsLeft;

	public float Damage {
		get { return damage; }
	}
	public int BulletsLeft {
		get { return bulletsLeft; }
		set { bulletsLeft = value; }
	}

	bool firing = false;

	void Start() {
		count = bulletsLeft;
	}

	public override void UseEquipment () {
		if (bullet) {
			if (!firing) {
				firing = true;
				StartCoroutine ("Shoot");
			}
		}
	}

	IEnumerator Shoot() {
		for (int i = 0; i < roundsPerFire; i++) {
			if (bulletsLeft > 0 || bulletsLeft == -1) {
				Transform parent = transform.parent.gameObject.transform;
				Vector3 barrelPos = parent.position + parent.forward * barrelDist;
				GameObject firedBullet = (GameObject)Instantiate (bullet, barrelPos, Quaternion.AngleAxis (90, transform.right));
				Rigidbody rb = firedBullet.GetComponent<Rigidbody> ();
				Bullet b = firedBullet.GetComponent<Bullet> ();
				if (rb && b) {
					b.Damage = damage;
					firedBullet.layer = gameObject.layer;
					rb.velocity = parent.forward * bulletSpeed;
					if (bulletsLeft > 0) {
						bulletsLeft--;
						count = bulletsLeft;
					}
					yield return new WaitForSeconds (timeBetweenRounds);
				}
			}
		}
		firing = false;
	}
}
