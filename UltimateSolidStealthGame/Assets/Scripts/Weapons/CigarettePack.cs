using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CigarettePack : MonoBehaviour {

	[SerializeField]
	int location;

	//BoxCollider collider;
	Graph graph;

	public int Location {
		get { return location; }
		set { location = value; }
	}

	void Start () {
		transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
		transform.Rotate (new Vector3(0, Random.Range(0, 360), 0));
		graph = GameObject.FindGameObjectWithTag ("Graph").GetComponent<Graph> ();
		location = graph.GetIndexFromPosition (transform.position);
		//collider = GetComponent<BoxCollider> ();
	}
}
