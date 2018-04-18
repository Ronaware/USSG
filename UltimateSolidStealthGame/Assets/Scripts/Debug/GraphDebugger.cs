using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphDebugger : MonoBehaviour {

	public GameObject sphere;

	Graph graph;

	// Use this for initialization
	void Start () {
		graph = GetComponent<Graph> ();
		Vector3 offset = new Vector3 (0.0f, 1.5f, 0.0f);
		if (sphere != null && graph != null) {
			int count = 0;
			foreach (Vertex v in graph.vertices) {
				if (v != null) {
					GameObject temp = GameObject.Instantiate (sphere, v.position + offset, Quaternion.identity);
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
	
	// Update is called once per frame
	void Update () {
		
	}
}
