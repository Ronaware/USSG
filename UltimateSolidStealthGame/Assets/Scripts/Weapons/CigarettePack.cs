using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class of CigarettePack dropped by Cigarette 
	The actual distraction object
*/
public class CigarettePack : MonoBehaviour {

	/*
		index of vertex this pack is at
	*/
	[SerializeField]
	int location;

	/*
		Reference to Graph component of Graph gameobject in scene
	*/
	//Graph graph;

	public int Location {
		get { return location; }
		set { location = value; }
	}

	void Start () {
		//transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);
		transform.Rotate (new Vector3(0, Random.Range(0, 360), 0));
		//graph = GameObject.FindGameObjectWithTag ("Graph").GetComponent<Graph> ();
		//location = graph.GetIndexFromPosition (transform.position);
	}
}
