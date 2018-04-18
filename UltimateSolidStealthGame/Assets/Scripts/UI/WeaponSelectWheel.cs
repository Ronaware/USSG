using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectWheel : MonoBehaviour {

	float wheelRadius;

	Image bgCircle;
	List<GameObject> buttonList;
	int lastListSize;
	float midX;
	float midY;

	// Use this for initialization
	void Awake () {
		bgCircle = GetComponent<Image> ();
		bgCircle.gameObject.SetActive (false);
		buttonList = new List<GameObject> ();
		wheelRadius = Screen.height / 3.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddButton(GameObject newButton) {
			buttonList.Add (newButton);
	}

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

	public void HideWheel() {
		foreach (GameObject g in buttonList) {
			g.SetActive (false);
		}
		bgCircle.gameObject.SetActive (false);
	}
}
