using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

	[SerializeField]
	int currVertexIndex;
	[SerializeField]
	public int pauseLength = 10;

	int lastVertexIndex;
	bool alerted;
	int destPatrolIndex;
	Graph graph;
	UnityEngine.AI.NavMeshAgent nav;
	List<int> path;
	Enums.directions direction;
	string enemyName;
	Vector3 lastMoveDir;
	GameObject player;

	public List<int> patrolVertices = new List<int> ();
	public int CurrVertexIndex {
		get { return currVertexIndex; }
	}
	public List<int> Path {
		set { path = value; }
	}
	public bool Alerted {
		get { return alerted; }
		set { alerted = value; }
	}

	// Use this for initialization
	void Start() {
		alerted = false;
		GameObject temp = GameObject.FindGameObjectWithTag ("Graph");
		if (temp) {
			graph = temp.GetComponent<Graph> ();
			if (graph) {
				nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
				path = new List<int> ();
				transform.position.Set (transform.position.x, 0.0f, transform.position.z);
				currVertexIndex = graph.GetIndexFromPosition (transform.position);
				lastVertexIndex = currVertexIndex;
				lastMoveDir = transform.forward;
				enemyName = gameObject.name;
				graph.vertices [currVertexIndex].occupiedBy = enemyName;
				graph.vertices [currVertexIndex].occupied = true;
				player = GameObject.FindGameObjectWithTag ("Player");
				if (patrolVertices.Count > 0) {
					path = graph.FindShortestPath (currVertexIndex, patrolVertices [0]);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update() {
		if (graph.Ready == true) {
			if (alerted == true) {
				if (path.Count == 0 && patrolVertices.Count > 0) {
					alerted = false;
					StartCoroutine ("Pause", currVertexIndex);
				} else if (path.Count == 2 && player != null) {
					if (Vector3.Distance(player.transform.position, transform.position) == graph.VertexDistance) {
						Vector3 moveDir = player.transform.position - transform.position;
						StartCoroutine ("TurnDownPath", moveDir);
					}
				}
			}
			TravelBetweenPathPoints ();
			OnPatrol ();
		} 
	}

	void OnPatrol() {
		if (patrolVertices.Count > 1) {
			int patrolIndexInGraph = patrolVertices [destPatrolIndex];
			Vertex v = graph.vertices[patrolIndexInGraph];
			float currX = transform.position.x;
			float currZ = transform.position.z;
			float destX = v.position.x;
			float destZ = v.position.z;
			if (currX == destX && currZ == destZ) {
				destPatrolIndex = (destPatrolIndex + 1) % patrolVertices.Count;
				path = graph.FindShortestPath (patrolIndexInGraph, patrolVertices[destPatrolIndex]);
				StartCoroutine ("Pause", patrolIndexInGraph);
			}
		}
	}

	void TravelBetweenPathPoints() {      
		if (path != null && path.Count > 0) {
			Vertex v = graph.vertices[path[0]];
			float currX = transform.position.x;
			float currZ = transform.position.z;
			float destX = v.position.x;
			float destZ = v.position.z;
			if (nav.remainingDistance <= 0.1f) {
				if (lastVertexIndex != currVertexIndex) {
					if (graph.vertices[lastVertexIndex].occupiedBy == enemyName) {
						graph.vertices [lastVertexIndex].occupied = false;
						graph.vertices [lastVertexIndex].occupiedBy = "";
					}
					graph.vertices [currVertexIndex].occupied = true;
					graph.vertices [currVertexIndex].occupiedBy = enemyName;
				}
				if (path.Count > 1) {
                    if (graph.vertices[path[1]].occupied == true) { 
						return;
					}
				}
				path.RemoveAt (0);
				Vector3 moveDir;
				if (path.Count > 0 && nav != null) {
					lastVertexIndex = currVertexIndex;
					moveDir = graph.vertices [path [0]].position - graph.vertices [currVertexIndex].position;
					if (moveDir != lastMoveDir) {
						StartCoroutine ("TurnDownPath", moveDir);
					}
					lastMoveDir = moveDir;
					if (moveDir.x > 0) {
						currVertexIndex += 1;
					} else if (moveDir.x < 0) {
						currVertexIndex -= 1;
					} else if (moveDir.z > 0) {
						currVertexIndex += graph.GridWidth;
					} else if (moveDir.z < 0) {
						currVertexIndex -= graph.GridWidth;
					}
					graph.vertices [lastVertexIndex].occupied = true;
					graph.vertices [lastVertexIndex].occupiedBy = enemyName;
					graph.vertices [currVertexIndex].occupied = true;
					graph.vertices [currVertexIndex].occupiedBy = enemyName;
					nav.SetDestination (graph.vertices [path [0]].position);
				}
			}
		}
	}

	IEnumerator Pause(int startIndex) {
		enabled = false;
		for (int i = 0; i < pauseLength; i++) {
			if (alerted == false) {
				yield return new WaitForSeconds (0.1f);
			} else {
				enabled = true;
				yield break;
			}
		}
		path = graph.FindShortestPath (startIndex, patrolVertices [destPatrolIndex]);
		enabled = true;
	}

	IEnumerator TurnDownPath(Vector3 towards) {
		int count = 0;
		float angle = Vector3.SignedAngle (transform.forward.normalized, towards.normalized, Vector3.up);
		while (Vector3.Angle(transform.forward.normalized, towards.normalized) != 0.0f && count <= 8) {
			if (angle < 0.0f) {
				transform.rotation *= Quaternion.Euler (0.0f, -22.5f, 0.0f);
			} else {
				transform.rotation *= Quaternion.Euler (0.0f, 22.5f, 0.0f);
			}
			count++;
			yield return null;
		}
		transform.rotation = Quaternion.LookRotation(towards);
		yield return null;
	}
}
