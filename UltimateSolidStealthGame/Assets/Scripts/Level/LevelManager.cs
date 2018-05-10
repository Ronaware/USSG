using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	[SerializeField]
	string levelName;
	[SerializeField]
	int level;
	[SerializeField]
	List<GameObject> equipmentForLevel;
	[SerializeField]
	string nextLevel;

	PlayerManager manager;

	public string LevelName {
		get { return LevelName; }
	}

	void Awake () {
		manager = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerManager> ();
		foreach (GameObject e in equipmentForLevel) {
			manager.WeaponSystem.AddEquipment (e);
		}
		int currLevel = PlayerPrefs.GetInt ("CurrentLevel", -1);
		if (currLevel != -1) {
			if (currLevel < level) {
				Time.timeScale = 0.0f;

				//Play level introduction ui animation
			}
		}
	}

	void Update () {}

	public void FinishLevel() {
		//play finish level ui animation
		//Set new playerpref currentlevel
	}
}
