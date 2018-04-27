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
	int destPatrolIndex;
	List<int> path;
	Enums.directions direction;
	string enemyName;
	Vector3 lastMoveDir;
	bool turning;

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
		get { return path; }
		set { path = value; }
	}
	public UnityEngine.AI.NavMeshAgent Nav {
		get { return nav; }
	}

	void Start() {
		manager = GetComponent<EnemyManager> ();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		path = new List<int> ();
		transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
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

	void Update() {
		if (manager && manager.Graph.Ready) {
			if (!manager.Sight.Alerted) {
				if (manager.Distraction && !manager.Distraction.Distracted) {
					BackToPatrol ();
				} else if (!manager.Distraction) {
					BackToPatrol ();
				}
			}
			TravelBetweenPathPoints ();
			OnPatrol ();


			foreach(int i in path) {
				Vector3 pos = manager.Graph.vertices [i].position;
				Debug.DrawLine (pos, pos + Vector3.up, Color.red, 0.01f);
			}
		} 
	}

	void OnPatrol() {
		if (!manager.Sight.Alerted && patrolVertices.Count > 1) {
			int patrolIndexInGraph = patrolVertices [destPatrolIndex];
			Vertex v = manager.Graph.vertices[patrolIndexInGraph];
			float currX = transform.position.x;
			float currZ = transform.position.z;
			float destX = v.position.x;
			float destZ = v.position.z;
			if (Mathf.Approximately(currX, destX) && Mathf.Approximately(currZ, destZ)) {
				destPatrolIndex = (destPatrolIndex + 1) % patrolVertices.Count;
				StartCoroutine ("Pause");
			}
		}
	}

	void TravelBetweenPathPoints() {      
		if (path.Count > 0) {
			Vertex v = manager.Graph.vertices [path [0]];
			float currX = transform.position.x;
			float currZ = transform.position.z;
			float destX = v.position.x;
			float destZ = v.position.z;
			if (nav.remainingDistance <= 0.1f) {
				if (lastVertexIndex != currVertexIndex) {
					if (manager.Graph.vertices [lastVertexIndex].occupiedBy == enemyName) {
						manager.Graph.vertices [lastVertexIndex].occupied = false;
						manager.Graph.vertices [lastVertexIndex].occupiedBy = "";
					}
					manager.Graph.vertices [currVertexIndex].occupied = true;
					manager.Graph.vertices [currVertexIndex].occupiedBy = enemyName;
				}
				Vector3 moveDir;
				if (path.Count >= 2) {
					if (manager.Graph.vertices [path [1]].occupied) { 
						moveDir = manager.Graph.vertices [path [1]].position - manager.Graph.vertices [path [0]].position;
						if (moveDir != lastMoveDir) {
							StartCoroutine ("TurnDownPath", moveDir);
							lastMoveDir = moveDir;
						}
						return;
					}
				}
				path.RemoveAt (0);
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

	public void BackToPatrol() {
		List<int> newPath = manager.Graph.FindShortestPath (currVertexIndex, patrolVertices [destPatrolIndex]);
		if (newPath.Count > 0) {
			path = newPath;
		}
	}

	public void PauseMovement() {
		StartCoroutine ("Pause");
	}

	IEnumerator Pause() {
		enabled = false;
		for (int i = 0; i < pauseLength; i++) {
			if (!manager.Sight.Alerted) {
				yield return new WaitForSeconds (0.1f);
			} else if (manager.Sight.Alerted || (manager.Distraction && manager.Distraction.Distracted) || path.Count != 0){
				enabled = true;
				yield break;
			}
		}
		if (path.Count == 0) {
			BackToPatrol ();
		}
		enabled = true;
	}

	public void Turn(Vector3 dir) {
		StartCoroutine ("TurnDownPath", dir);
	}

	IEnumerator TurnDownPath(Vector3 towards) {
		if (!turning) {
			turning = true;
			int count = 0;
			float angle = Vector3.SignedAngle (transform.forward.normalized, towards.normalized, Vector3.up);
			while (Mathf.Abs(Vector3.Angle (transform.forward.normalized, towards.normalized)) >= 15.0f && count <= 8) {
				if (angle < 0.0f) {
					transform.rotation *= Quaternion.Euler (0.0f, -22.5f, 0.0f);
				} else {
					transform.rotation *= Quaternion.Euler (0.0f, 22.5f, 0.0f);
				}
				count++;
				yield return null;
			}
			transform.rotation = Quaternion.LookRotation (towards);
			turning = false;
		}
	}
}
