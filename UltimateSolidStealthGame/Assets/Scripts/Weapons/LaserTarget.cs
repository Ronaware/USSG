using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTarget : MonoBehaviour {

	[SerializeField]
	int location;

	Graph graph;
	LaserPointer pointer;

	public int Location {
		get { return location; }
		set { location = value; }
	}
	public LaserPointer Pointer {
		set { pointer = value; }
	}
		
	void Start () {
		//transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
		graph = GameObject.FindGameObjectWithTag ("Graph").GetComponent<Graph> ();
		location = graph.GetIndexFromPosition (transform.position);
	}

	void OnDestroy() {
		if (pointer) {
			pointer.LaserInvestigated ();
		}
	}
}
