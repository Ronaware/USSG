using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

	protected HealthManager health;
	protected Graph graph;

	public HealthManager Health {
		get { return health; }
	}
	public Graph Graph {
		get { return graph; }
	}
		
	protected virtual void Awake () {
		health = GetComponent<HealthManager> ();
		GameObject tempGraph = GameObject.FindGameObjectWithTag ("Graph");
		if (tempGraph) {
			graph = tempGraph.GetComponent<Graph> ();
		}
	}
	
	public virtual void Kill() {}
}
