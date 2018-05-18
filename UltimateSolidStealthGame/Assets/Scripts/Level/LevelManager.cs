using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    int playerCurrLevel;

	public string LevelName {
		get { return LevelName; }
	}

	void Awake () {
		manager = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerManager> ();
		foreach (GameObject e in equipmentForLevel) {
			manager.WeaponSystem.AddEquipment (e);
		}
		playerCurrLevel = PlayerPrefs.GetInt ("CurrentLevel", -1);
		if (playerCurrLevel != -1) {
			if (playerCurrLevel < level) {
				

				//Play level introduction ui animation
			}
		}
	}

	void Update () {}

	public void FinishLevel() {
        //play finish level ui animation
        //Set new playerpref currentlevel
        if (nextLevel != "") {
            if (playerCurrLevel < level) {
				PlayerPrefs.SetInt ("CurrentLevel", level);
            }
			SceneManager.LoadScene(nextLevel);
        }
	}
}
