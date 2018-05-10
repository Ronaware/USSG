using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class used by enemy to detect any distractions
	If distraction is detected, a path to it is set
*/
public abstract class EnemyDistraction : MonoBehaviour {

	/*
		radius around enemy in which to detect distraction
	*/
	[SerializeField]
	protected float checkRadius;
	/*
		time between Physics.CheckSphere calls to find distractions
	*/
	[SerializeField]
	protected float timeBetweenChecks;
	/*
		time the enemy should be distracted when they reach a distraction
	*/
	[SerializeField]
	protected float distractionTime;

	/*
		reference to the distraction if there is one detected
	*/
	protected GameObject distraction;
	/*
		whether the enemy is currently distracted
	*/
	protected bool distracted;
	/*
		reference to EnemyManager component
	*/
	protected EnemyManager manager;
	/*
	 	list used to store indices of vertices that make up path to distraction
	*/
	protected List<int> pathToDistraction;
	/*
	 	layer that defines specific distraction objects
	*/
	protected int distractionLayers;

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

	/*
	 	Called at the end of every frame
	 	if enemy is at distraction, start AtDistraction coroutine
	 	if distraction is destroyed before enemy gets to it, go back to patrolling
	*/
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

	/*
	 	uses Physics.CheckSphere to check surrounding area for distractions
	*/
	protected abstract void CheckForDistraction ();
		
	/*
	 	Sets the distraction and finds path to it if one is detected
		@param vertex - vertex that distraction is at
		@param obj - reference to distraction object
	*/
	public virtual void SetDistraction (int vertex, ref GameObject obj) {
		if (!manager.Sight.Alerted && vertex >= 0) {
			distraction = obj;
			distracted = true;
			pathToDistraction = manager.Graph.FindShortestPath (manager.Movement.CurrVertexIndex, vertex);
			if (pathToDistraction.Count > 0) {
				manager.Movement.Path = pathToDistraction;
			}
		}
	}

	//TODO if enemy is distracted, does the sightplane go away?
	/*
	 	coroutine that defines what enemy should do once they reach the distraction
	*/
	protected abstract IEnumerator AtDistraction () ;
}
