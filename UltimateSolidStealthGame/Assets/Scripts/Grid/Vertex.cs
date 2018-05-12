using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex {

	public Vector3 position = new Vector3 ();
	public bool visited;
	public List<int> adjacentVertices = new List<int> ();
	public int index;
	public bool occupied;
	public string occupiedBy;
}
