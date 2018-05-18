using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex {

	public Vector3 position = new Vector3();
	public bool visited;
	public List<int> adjacentVertices = new List<int>();
    public List<Vertex> childVertices = new List<Vertex>();
    public Vertex parentVertex = null;
    public int index;
	public bool occupied;
	public string occupiedBy;

    public void NotifyParentOrChild() {
        if (childVertices.Count > 0) {
            foreach (Vertex v in childVertices) {
                v.occupied = occupied;
                v.occupiedBy = occupiedBy;
            }
        } else if (parentVertex != null) {
            parentVertex.occupied = occupied;
            parentVertex.occupiedBy = occupiedBy;
        }

    }
}
