using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDistraction : MonoBehaviour {

	[SerializeField]
	protected float checkRadius;
	[SerializeField]
	protected float timeBetweenChecks;
	[SerializeField]
	protected GameObject distraction;
	[SerializeField]
	protected bool distracted;
	[SerializeField]
	protected float distractionTime;

	protected EnemyManager manager;
	protected List<int> pathToDistraction;
	protected int nonDistractionLayers;

	public EnemyManager Manager {
		get { return manager; }
	}
	public bool Distracted {
		get { return distracted; }
		set { distracted = value; }
	}

	public virtual void Start() {
		manager = GetComponent<EnemyManager> ();
		pathToDistraction = new List<int> ();
		InvokeRepeating ("CheckForDistraction", 0.0f, timeBetweenChecks);
	}

	protected virtual void LateUpdate () {
		if (distracted) {
			if (distraction && pathToDistraction.Count == 0) { 
				Vector3 pos = distraction.transform.position;
				if (Mathf.Approximately(transform.position.x, pos.x) && Mathf.Approximately(transform.position.z, pos.z)) {
					enabled = false;
					manager.Movement.enabled = false;
					manager.Sight.enabled = false;
					manager.WeaponSystem.enabled = false;
					StartCoroutine ("AtDistraction");
				}
			} else if (!distraction) {
				distracted = false;
				pathToDistraction.Clear ();
				manager.Movement.BackToPatrol ();
				return;
			}
		}
	}

	protected abstract void CheckForDistraction ();
		
	public abstract void SetDistraction (int vertex, ref GameObject obj);

	protected abstract IEnumerator AtDistraction () ;
}
