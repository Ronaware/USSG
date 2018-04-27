using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySight : MonoBehaviour {

	[SerializeField]
	protected int sightDistance = 6;
	[SerializeField]
	protected int FOV = 45;
	[SerializeField]
	protected int alertedFOV = 150;
	[SerializeField]
	protected int numFramesToResetPath = 10;
	[SerializeField]
	protected bool alerted;

	protected int frames;
	protected PlayerMovement playerMovement;
	protected EnemyManager manager;
	protected int ignoreEnemiesLayer;
	protected List<int> pathToPlayer;

	public int IgnoreEnemiesLayer {
		get { return ignoreEnemiesLayer; }
	}
	public bool Alerted {
		get { return alerted; }
		set { alerted = value; }
	}
		
	protected virtual void Start () {
		frames = 0;
		ignoreEnemiesLayer = 1 << LayerMask.NameToLayer ("Enemy");
		ignoreEnemiesLayer = ~ignoreEnemiesLayer;
		GameObject temp = GameObject.FindGameObjectWithTag ("Player");
		if (temp) {
			playerMovement = temp.GetComponent<PlayerMovement> ();
		}
		manager = gameObject.GetComponent<EnemyManager> ();
		pathToPlayer = new List<int> ();
	}

	protected virtual void Update() {
		CheckSightline ();
		if (alerted && pathToPlayer.Count == 0) {
			alerted = false;
			manager.Movement.PauseMovement ();
		}
	}

	protected abstract void CheckSightline ();

}
