using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	Class that stores all of the player's equipment and allows them to swap and use the current weapon
*/
public class PlayerWeaponSystem : MonoBehaviour {
	/*
		list of all weapon instances
	*/
	List<GameObject> equipmentInstances;
	/*
		type of weapon currently equipped
	*/
	string currEquipmentType;
	/*
		reference to current equipment object
	*/
	GameObject currEquipment;
	/*
		reference to Equipment component of currEquipment
	*/
	Equipment equipment;

	/*
		reference to PlayerManager component
	*/
	PlayerManager manager;

	public string CurrEquipmentType {
		get { return currEquipmentType; }
	}
	public GameObject CurrEquipment {
		get { return currEquipment; }
	}
	public Equipment Equipment {
		get { return equipment; }
	}
		
	void Awake() {
		equipmentInstances = new List<GameObject> ();
		manager = GetComponent<PlayerManager> ();
	}

	void Start() {
		SwapEquipment ("Knife");
	}

	/*
		Used to detect collision with other collider
		if collider has component that can be picked up, add to equipment
		@param collision - info on collider that was collided with
	*/
	void OnCollisionEnter(Collision collision) {
		//use for gun pickup
	}

	/*
		use current equipment
	*/
	public void UseEquipped() {
		if (equipment) {
			equipment.UseEquipment ();
		}
	}

	/*
		swaps to equipment in list of type eType
		@param eType - type of equipment to switch to
	*/
	public void SwapEquipment(string eType) {
		if (eType != CurrEquipmentType) {
			if (currEquipment) {
				currEquipment.GetComponent<Equipment>().EquipmentRender(false);
			}
			foreach (GameObject g in equipmentInstances) {
				Equipment e = g.GetComponent<Equipment> ();
				if (e.EquipmentType == eType) {
					e.EquipmentRender(true);
					currEquipment = g;
					equipment = currEquipment.GetComponent<Equipment> ();
					currEquipmentType = equipment.EquipmentType;
					manager.Ui.UpdateUIOnGunSwap (equipment, currEquipmentType);
					break;
				}
			}
		}
	}

	/*
		add instance of equipment to list of available equipment
		@param newPrefab - prefab of equipment to be instantiated and added
	*/
	public void AddEquipment(GameObject newPrefab) {
		GameObject temp = GameObject.Instantiate (newPrefab, transform);
		temp.layer = LayerMask.NameToLayer ("PlayerWeapon");
		temp.GetComponent<Equipment>().EquipmentRender(false);
		equipmentInstances.Add (temp);
		manager.Ui.AddEquipment (ref temp);
	}
}
