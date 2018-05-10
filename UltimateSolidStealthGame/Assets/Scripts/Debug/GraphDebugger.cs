using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphDebugger : MonoBehaviour {

	public GameObject sphere;

	Graph graph;

	void Start () {
		graph = GetComponent<Graph> ();
		if (sphere != null && graph != null) {
			int count = 0;
			foreach (Vertex v in graph.vertices) {
				if (v != null) {
					GameObject temp = GameObject.Instantiate (sphere, v.position, Quaternion.identity);
					if (temp != null) {
						VertexDisplay vertDisplay = temp.GetComponent<VertexDisplay> ();
						if (vertDisplay) {
							vertDisplay.vert = graph.vertices[count];
						}
					}
				}
				count++;
			}
		}
	}

	void Update () {
		
	}
}
