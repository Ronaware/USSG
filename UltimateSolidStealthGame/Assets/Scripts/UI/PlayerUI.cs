using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	GameObject buttonPrefab;
	[SerializeField]
	Text counter;
	[SerializeField]
	GameObject weaponWheelPrefab;

	WeaponSelectWheel weaponWheel;
	List<GameObject> equipmentInstanceList;
	float pressTime;
	PlayerWeaponSystem playerWeaponSystem;
	HealthManager playerHealth;
	string currEquipmentType;
	Equipment currEquipment;
	bool wheelDisplayed;
	bool primaryPressed;

	void Awake () {
		equipmentInstanceList = new List<GameObject> ();
		GameObject temp = GameObject.FindGameObjectWithTag ("Player");
		if (temp) {
			playerWeaponSystem = temp.GetComponent<PlayerWeaponSystem> ();
			playerHealth = temp.GetComponent<HealthManager> ();
			if (playerWeaponSystem) {
				currEquipmentType = playerWeaponSystem.CurrEquipmentType;
				currEquipment = playerWeaponSystem.Equipment;
				temp = GameObject.Instantiate (weaponWheelPrefab);
				temp.transform.parent = gameObject.transform;
				RectTransform rect = temp.GetComponent<RectTransform> ();
				rect.position = new Vector3 (Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
				rect.localScale = Vector3.one;
				weaponWheel = temp.GetComponent<WeaponSelectWheel> ();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (currEquipment) {
			counter.text = "x" + currEquipment.Count.ToString ();
		}
		if (primaryPressed == true) {
			float time = Time.time - pressTime;
			if (time > 0.35f) {
				primaryPressed = false;
				weaponWheel.DisplayWheel();
				wheelDisplayed = true;
			}
		}
	}

	public void AddEquipment(ref GameObject e) {
		equipmentInstanceList.Add (e);
		Equipment equipment = e.GetComponent<Equipment> ();
		GameObject newEquipment = Instantiate (buttonPrefab);
		Button button = newEquipment.GetComponent<Button> ();
		Image image = newEquipment.GetComponent<Image> ();
		RectTransform rect = newEquipment.GetComponent<RectTransform> ();
		button.onClick.AddListener (delegate{playerWeaponSystem.SwapEquipment(equipment.EquipmentType);});
		image.sprite = equipment.Icon;
		weaponWheel.AddButton (newEquipment);
		newEquipment.transform.parent = weaponWheel.transform;
		rect.localPosition = Vector3.one;
		rect.localScale = Vector3.one;
		newEquipment.SetActive (false);
	}

	public void OnPrimaryDown() {
		if (wheelDisplayed) {
			weaponWheel.HideWheel();
			wheelDisplayed = false;
		} else {
			pressTime = Time.time;
			primaryPressed = true;
		}
	}

	public void OnPrimaryUp() {
		if (primaryPressed) {
			playerWeaponSystem.UseEquipped ();
			primaryPressed = false;
		}
	}

	public void UpdateUIOnGunSwap(Equipment e, string s) {
		currEquipmentType = s;
		currEquipment = e;
		weaponWheel.HideWheel ();
		wheelDisplayed = false;
	}
}
