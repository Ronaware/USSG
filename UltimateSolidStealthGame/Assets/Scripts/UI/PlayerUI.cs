using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
	Class to display and update the UI and allow user interaction
*/
public class PlayerUI : MonoBehaviour {

	/*
		prefab for button to be created dynamically
	*/
	[SerializeField]
	GameObject buttonPrefab;
	/*
		text object to display gun bullet count
	*/
	[SerializeField]
	Text counter;
	/*
		slider to display ice machine usage
	*/
	[SerializeField]
	Slider slider;
	/*
		reference to WeaponWheel object prefab
	*/
	[SerializeField]
	GameObject weaponWheelPrefab;

	/*
		reference to WeaponSelectWheel component of weaponwheel object
	*/
	WeaponSelectWheel weaponWheel;
	/*
		reference to all Equipment instances to make appropriate button
	*/
	List<GameObject> equipmentInstanceList;
	/*
		time the main button was pressed
	*/
	float pressTime;
	/*
		reference to PlayerMAnager component
	*/
	PlayerManager manager;
	/*
		type of currently equipped Equipment
	*/
	string currEquipmentType;
	/*
		reference to Equipment component of currEquipment game object
	*/
	Equipment currEquipment;
	/*
		whether the weaponwheel is displayed
	*/
	bool wheelDisplayed;
	/*
		whether the main button is being pressed
	*/
	bool primaryPressed;

	void Awake () {
		equipmentInstanceList = new List<GameObject> ();
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if (player) {
			manager = player.GetComponent<PlayerManager> ();
			if (manager) {
				GameObject temp = GameObject.Instantiate (weaponWheelPrefab);
				temp.transform.SetParent(gameObject.transform, false);
				RectTransform rect = temp.GetComponent<RectTransform> ();
				rect.position = new Vector3 (Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
				rect.localScale = Vector3.one;
				weaponWheel = temp.GetComponent<WeaponSelectWheel> ();
			}
		}
	}

	void Update () {
		if (currEquipment) {
			if (counter.IsActive()) {
				counter.text = "x" + currEquipment.Count.ToString ();
			} else if (slider.IsActive()) {
				slider.value = currEquipment.Count / 100.0f;
			}
			if (primaryPressed) {
				float time = Time.time - pressTime;
				if (time > 0.5f) {
					primaryPressed = false;
					weaponWheel.DisplayWheel();
					wheelDisplayed = true;
				}
			}
		}
	}

	/*
		add equipment to equipmentInstanceList
		create new button and add to wheel for equipment
		@param e - reference to instance of new Equipment object
	*/
	public void AddEquipment(ref GameObject e) {
		equipmentInstanceList.Add (e);
		Equipment equipment = e.GetComponent<Equipment> ();
		GameObject newEquipment = Instantiate (buttonPrefab);
		Button button = newEquipment.GetComponent<Button> ();
		Image image = newEquipment.GetComponent<Image> ();
		button.onClick.AddListener (delegate{manager.WeaponSystem.SwapEquipment(equipment.EquipmentType);});
		image.sprite = equipment.Icon;
		weaponWheel.AddButton (newEquipment);
	}

	/*
		called when main button is pressed (setup in inspector)
		if wheel is displayed, hide wheel
		else start checking presstime to either use equipment or display wheel
	*/
	public void OnPrimaryDown() {
		if (wheelDisplayed) {
			weaponWheel.HideWheel();
			wheelDisplayed = false;
		} else {
			pressTime = Time.time;
			primaryPressed = true;
		}
	}

	/*
	 	called when main button is released (setup in inspector)
		if primaryPressed, use equipment
	*/
	public void OnPrimaryUp() {
		if (primaryPressed) {
			manager.WeaponSystem.UseEquipped ();
			primaryPressed = false;
		}
	}

	/*
		updates the ui when equipped weapon is switched
		displayes counter or slider based on Equipment subclass type
	*/
	public void UpdateUIOnGunSwap(Equipment e, string s) {
		currEquipmentType = s;
		currEquipment = e;
		weaponWheel.HideWheel ();
		wheelDisplayed = false;
		if (e.GetComponent<Gun>() || e.GetComponent<Cigarette>()) {
			counter.gameObject.SetActive(true);
			slider.gameObject.SetActive(false);
		} else if (e.GetComponent<IceMachine>()) {
			counter.gameObject.SetActive(false);
			slider.gameObject.SetActive(true);
		} else {
			counter.gameObject.SetActive(false);
			slider.gameObject.SetActive(false);
		}
	}
}
