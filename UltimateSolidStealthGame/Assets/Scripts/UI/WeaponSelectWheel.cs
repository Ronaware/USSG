using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	Class to handle displaying and formatting WeaponSelection button wheel
*/
public class WeaponSelectWheel : MonoBehaviour {

	/*
		radius of button wheel
	*/
	float wheelRadius;

	/*
		reference to background circle image
	*/
	Image bgCircle;
	/*
		list of references to Button gameobjects
	*/
	List<GameObject> buttonList;
	/*
		number of buttons in list when last displayed
	*/
	int lastListSize;
	/*
		coordinates of middle of screen
	*/
	float midX;
	float midY;

	void Awake () {
		bgCircle = GetComponent<Image> ();
		bgCircle.gameObject.SetActive (false);
		buttonList = new List<GameObject> ();
		wheelRadius = Screen.height / 3.0f;
	}

	void Update () {
		
	}

	/*
		add button reference to list
		@param newButton - reference to Button gameobject
	*/
	public void AddButton(GameObject newButton) {
		buttonList.Add (newButton);
		newButton.transform.SetParent(transform, false);
		RectTransform rect = newButton.GetComponent<RectTransform> ();
		rect.localPosition = Vector3.one;
		rect.localScale = Vector3.one;
		newButton.SetActive (false);
	}

	/*
		displays wheel and sets button positions based on number of buttons in list
	*/
	public void DisplayWheel() {
		if (lastListSize != buttonList.Count) {
			lastListSize = buttonList.Count;
			float angle = (360.0f / buttonList.Count) * Mathf.Deg2Rad;
			for (int i = 0; i < lastListSize; i++) {
				buttonList [i].SetActive (true);
				float x = Mathf.Sin (angle * i) * wheelRadius;
				float y = Mathf.Cos (angle * i) * wheelRadius;
				Vector3 pos = new Vector3 (x, y, 0.0f) + transform.position;
				RectTransform rect = buttonList [i].GetComponent<RectTransform> ();
				rect.position = pos;
			}
		}
		foreach (GameObject g in buttonList) {
			g.SetActive (true);
		}
		bgCircle.gameObject.SetActive (true);
	}

	/*
		hides wheel
	*/
	public void HideWheel() {
		foreach (GameObject g in buttonList) {
			g.SetActive (false);
		}
		bgCircle.gameObject.SetActive (false);
	}
}
