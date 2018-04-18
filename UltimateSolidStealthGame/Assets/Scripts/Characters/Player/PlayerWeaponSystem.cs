using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponSystem : MonoBehaviour {

	[SerializeField]
	List<GameObject> equipmentPrefabs;

	List<GameObject> equipmentInstances;
	PlayerUI ui;
	string currEquipmentType;
	GameObject currEquipment;
	Equipment equipment;

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
		GameObject temp;
		foreach (GameObject g in equipmentPrefabs) {
			temp = GameObject.Instantiate (g, transform);
			temp.gameObject.SetActive (false);
			equipmentInstances.Add (temp);
		}
		temp = GameObject.FindGameObjectWithTag ("UI");
		if (temp) {
			ui = temp.GetComponent<PlayerUI> ();
			if (ui) {
				for (int i = 0; i < equipmentInstances.Count; i++) {
					GameObject g = equipmentInstances [i];
					ui.AddEquipment (ref g);
				}
			}
		}
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
				ui.UpdateUIOnGunSwap (equipment, currEquipmentType);
				break;
			}
		}
	}

	public void AddEquipment(GameObject newPrefab) {
		GameObject temp = GameObject.Instantiate (newPrefab, transform);
		temp.gameObject.SetActive (false);
		equipmentInstances.Add (temp);
		GameObject temp2 = GameObject.FindGameObjectWithTag ("UI");
		if (temp2) {
			ui = temp2.GetComponent<PlayerUI> ();
			if (ui) {
				ui.AddEquipment (ref temp);
			}
		}
	}
}
