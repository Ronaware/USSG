using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponSystem : MonoBehaviour {

	[SerializeField]
	List<GameObject> equipmentPrefabs;

	List<GameObject> equipmentInstances;
	string currEquipmentType;
	GameObject currEquipment;
	Equipment equipment;

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

	// Use this for initialization
	void Start () {
		equipmentInstances = new List<GameObject> ();
		manager = GetComponent<PlayerManager> ();
		GameObject temp;
		foreach (GameObject g in equipmentPrefabs) {
			AddEquipment(g);
		}
		SwapEquipment ("Knife");
	}

	void OnCollisionEnter(Collision collision) {
		//use for gun pickup
	}
	
	public void UseEquipped() {
		if (equipment) {
			equipment.UseEquipment ();
		}
	}

	public void SwapEquipment(string eType) {
		if (currEquipment) {
			currEquipment.gameObject.SetActive (false);
		}
		foreach (GameObject g in equipmentInstances) {
			Equipment e = g.GetComponent<Equipment> ();
			if (e.EquipmentType == eType) {
				g.SetActive (true);
				currEquipment = g;
				equipment = currEquipment.GetComponent<Equipment> ();
				currEquipmentType = equipment.EquipmentType;
				manager.Ui.UpdateUIOnGunSwap (equipment, currEquipmentType);
				break;
			}
		}
	}

	public void AddEquipment(GameObject newPrefab) {
		GameObject temp = GameObject.Instantiate (newPrefab, transform);
		temp.layer = LayerMask.NameToLayer ("PlayerWeapon");
		temp.gameObject.SetActive (false);
		equipmentInstances.Add (temp);
		manager.Ui.AddEquipment (ref temp);
	}
}
