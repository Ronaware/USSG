using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

	[SerializeField]
	int pauseLength = 10;
	[SerializeField]
	int currVertexIndex;

	int lastVertexIndex;
	bool alerted;
	int destPatrolIndex;
	List<int> path;
	Enums.directions direction;
	string enemyName;
	Vector3 lastMoveDir;

	EnemyManager manager;
	UnityEngine.AI.NavMeshAgent nav;

	public List<int> patrolVertices = new List<int> ();
	public int CurrVertexIndex {
		get { return currVertexIndex; }
	}
	public int LastVertexIndex {
		get { return lastVertexIndex; }
	}
	public List<int> Path {
		set { path = value; }
	}
	public bool Alerted {
		get { return alerted; }
		set { alerted = value; }
	}
	public UnityEngine.AI.NavMeshAgent Nav {
		get { return nav; }
	}

	// Use this for initialization
	void Start() {
		alerted = false;
		manager = GetComponent<EnemyManager> ();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		path = new List<int> ();
		transform.position.Set (transform.position.x, 0.0f, transform.position.z);
		currVertexIndex = manager.Graph.GetIndexFromPosition (transform.position);
		lastVertexIndex = currVertexIndex;
		lastMoveDir = transform.forward;
		enemyName = gameObject.name;
		manager.Graph.vertices [currVertexIndex].occupiedBy = enemyName;
		manager.Graph.vertices [currVertexIndex].occupied = true;
		if (patrolVertices.Count > 0) {
			path = manager.Graph.FindShortestPath (currVertexIndex, patrolVertices [0]);
		}
	}
	
	// Update is called once per frame
	void Update() {
		if (manager && manager.Graph.Ready) {
			if (alerted == true) {
				if (path.Count == 0 && patrolVertices.Count > 0) {
					alerted = false;
					StartCoroutine ("Pause", currVertexIndex);
				} else if (path.Count == 2 && manager.Player != null) {
					if (Vector3.Distance(manager.Player.transform.position, transform.position) == manager.Graph.VertexDistance) {
						Vector3 moveDir = manager.Player.transform.position - transform.position;
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
			Vertex v = manager.Graph.vertices[patrolIndexInGraph];
			float currX = transform.position.x;
			float currZ = transform.position.z;
			float destX = v.position.x;
			float destZ = v.position.z;
			if (currX == destX && currZ == destZ) {
				destPatrolIndex = (destPatrolIndex + 1) % patrolVertices.Count;
				path = manager.Graph.FindShortestPath (patrolIndexInGraph, patrolVertices[destPatrolIndex]);
				StartCoroutine ("Pause", patrolIndexInGraph);
			}
		}
	}

	void TravelBetweenPathPoints() {      
		if (path != null && path.Count > 0) {
			Vertex v = manager.Graph.vertices[path[0]];
			float currX = transform.position.x;
			float currZ = transform.position.z;
			float destX = v.position.x;
			float destZ = v.position.z;
			if (nav.remainingDistance <= 0.1f) {
				if (lastVertexIndex != currVertexIndex) {
					if (manager.Graph.vertices[lastVertexIndex].occupiedBy == enemyName) {
						manager.Graph.vertices [lastVertexIndex].occupied = false;
						manager.Graph.vertices [lastVertexIndex].occupiedBy = "";
					}
					manager.Graph.vertices [currVertexIndex].occupied = true;
					manager.Graph.vertices [currVertexIndex].occupiedBy = enemyName;
				}
				if (path.Count > 1) {
					if (manager.Graph.vertices[path[1]].occupied == true) { 
						return;
					}
				}
				path.RemoveAt (0);
				Vector3 moveDir;
				if (path.Count > 0 && nav != null) {
					lastVertexIndex = currVertexIndex;
					moveDir = manager.Graph.vertices [path [0]].position - manager.Graph.vertices [currVertexIndex].position;
					if (moveDir != lastMoveDir) {
						StartCoroutine ("TurnDownPath", moveDir);
					}
					lastMoveDir = moveDir;
					if (moveDir.x > 0) {
						currVertexIndex += 1;
					} else if (moveDir.x < 0) {
						currVertexIndex -= 1;
					} else if (moveDir.z > 0) {
						currVertexIndex += manager.Graph.GridWidth;
					} else if (moveDir.z < 0) {
						currVertexIndex -= manager.Graph.GridWidth;
					}
					manager.Graph.vertices [lastVertexIndex].occupied = true;
					manager.Graph.vertices [lastVertexIndex].occupiedBy = enemyName;
					manager.Graph.vertices [currVertexIndex].occupied = true;
					manager.Graph.vertices [currVertexIndex].occupiedBy = enemyName;
					nav.SetDestination (manager.Graph.vertices [path [0]].position);
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
		path = manager.Graph.FindShortestPath (startIndex, patrolVertices [destPatrolIndex]);
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
