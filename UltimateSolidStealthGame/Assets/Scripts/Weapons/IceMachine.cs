using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Subclass of Equipment that ices player and makes them invisible to bots
	Uses a fill bar to determine availability
*/
public class IceMachine : Equipment {

	/*
		how fast ice bar decreases
	*/
	[SerializeField]
	float iceTime;
	/*
		time to wait between ice machine turning off and refill start
	*/
	[SerializeField]
	float refillPauseTime;
	/*
		time it takes to refill ice
	*/
	[SerializeField]
	float refillTime;

	/*
		reference to player GameObject
	*/
	GameObject player;
	/*
		whether the player is currently iced
	*/
	bool playerIced;

	void Awake () {
		base.Awake ();
		player = transform.parent.gameObject;
		Count = 100;
	}

	/*
	 	base class override
		if playerIced == false, stop refilling and start icing
		else stop icing and start refilling
	*/
	public override void UseEquipment ()
	{
		if (!playerIced) {
			StopCoroutine ("Refill");
			StartCoroutine ("Ice");
		} else {
			StopCoroutine ("Ice");
			StartCoroutine ("Refill");
		}
	}

	/*
		coroutine to change player layer to iceplayer and make them invisible to bots
	*/
	IEnumerator Ice() {
		if (Count > 0) {
			playerIced = true;
			player.layer = LayerMask.NameToLayer ("IcePlayer");
			while (Count > 0) {
				yield return new WaitForSeconds (iceTime);
				Count--;
				if (render && !render.enabled) {
					break;
				}
			}
			StopCoroutine ("Refill");
			StartCoroutine ("Refill");
		}
	}

	/*
		coroutine to refill ice machine
	*/
	IEnumerator Refill() {
		playerIced = false;
		player.layer = LayerMask.NameToLayer ("Player");
		if (Count < 100) {
			yield return new WaitForSeconds (refillPauseTime);
			while (Count < 100) {
				Count++;
				yield return new WaitForSeconds (refillTime);
			}
		}
	}
}
