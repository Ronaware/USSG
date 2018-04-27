using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour{

	[SerializeField]
	int width;
	[SerializeField]
	int height;
	[SerializeField]
	float vertexDistance = 1.0f;

	int gridWidth;
	bool ready = false;

	public List<Vertex> vertices;
	public int GridWidth {
		get { return gridWidth; }
	}
	public bool Ready {
		get { return ready; }
	}
	public float VertexDistance {
		get { return vertexDistance; }
	}

	// Use this for initialization
	void Awake () {
		vertices = new List<Vertex> ();
		if (vertices.Count == 0) { 
			int count = 0;
			Vector3 pos = new Vector3 ();
			for (float i = vertexDistance; i < (float)height; i += vertexDistance) {                         
				for (float j = vertexDistance; j < (float)width; j += vertexDistance) {
					pos.Set (j, 0.5f, i);
					if (!Physics.CheckSphere (pos, 0.125f)) {
						Vertex vert = new Vertex ();
						vert.position.Set (pos.x, 0.0f, pos.z);
						vert.visited = false;
						vert.occupied = false;
						vert.index = count;
						vertices.Add (vert);
					} else {
						vertices.Add (null);
					}
					count++;
				}
			}
		}
		SetAdjacent ();
		ready = true;
	}

	void SetAdjacent() {
		int numVertices = vertices.Count;
		gridWidth = width * (int) (1.0f / vertexDistance) - 1;
		for (int i = 0; i < numVertices; i++) {
			if (vertices[i] != null && vertices[i].adjacentVertices.Count == 0) {
				if ((i - 1) >= 0 && ((i - 1) % gridWidth) < (i % gridWidth) && vertices[i - 1] != null) {
					vertices[i].adjacentVertices.Add (i - 1);
				}
				if ((i + 1) < numVertices && ((i + 1) % gridWidth) > (i % gridWidth) && vertices[i + 1] != null) {
					vertices[i].adjacentVertices.Add (i + 1);
				}
				if ((i - gridWidth) >= 0 && vertices[i - gridWidth] != null) {
					vertices[i].adjacentVertices.Add (i - gridWidth);
				}
				if ((i + gridWidth) < numVertices && vertices[i + gridWidth] != null) {
					vertices[i].adjacentVertices.Add (i + gridWidth);
				}
			}
		}
	}

	public List<int> FindShortestPath(int begin, int end) {
		Vertex source = vertices[begin];
		Vertex dest = vertices[end];
		Queue<List<int>> queue = new Queue<List<int>> ();
		foreach (Vertex v in vertices) {
			if (v != null) {
				v.visited = false;
			}
		}
		source.visited = true;
		List<int> path = new List<int> ();
		path.Add (source.index);
		queue.Enqueue (path);
		while (queue.Count != 0) {
			List<int> currPath = queue.Dequeue ();
			Vertex currVertex = vertices[currPath[currPath.Count - 1]];
			if (currVertex.position == dest.position) {
				return currPath;
			} else {
				foreach (int index in currVertex.adjacentVertices) {
					if (vertices[index].visited == false && (vertices[index].occupiedBy == "Player" || vertices[index].occupied == false)) {
						List<int> newPath = new List<int> (currPath);
						vertices [index].visited = true;
						newPath.Add (index);
						queue.Enqueue (newPath);
					}
				}
			}
		}
		return new List<int> ();
	}

	public int GetIndexFromPosition(Vector3 pos) {
		foreach (Vertex v in vertices) {
			if (v != null) {
				if (Mathf.Approximately(v.position.x, pos.x) && Mathf.Approximately(v.position.z, pos.z)) {
					return v.index;
				}
			}
		}
		return -1;
	}
}
