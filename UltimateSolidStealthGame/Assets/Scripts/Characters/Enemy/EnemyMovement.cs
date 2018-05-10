using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
	Class that allows enemies to move between points on graph
*/
public class EnemyMovement : MonoBehaviour {

	/*
		time enemy should pause when they reach patrol point
	*/
	[SerializeField]
	int pauseLength;
	/*
		current vertex in graph that enemy is at
	*/
	[SerializeField]
	int currVertexIndex;
	/*
		list of indices of vertices that the enemy should patrol between
	*/
	[SerializeField]
	List<int> patrolVertices = new List<int> ();
	/*
		used for debugging, shows path enemy is currently traveling along
	*/
	[SerializeField]
	bool showDebug;

	/*
		index of vertex that the enemy was last at
	*/
	int lastVertexIndex;
	/*
		index of patrol vertex that the enemy should move to next
	*/
	int destPatrolIndex;
	/*
		primary path that defines path along which the enemy moves, either between patrols, to distractions, or to player
	*/
	List<int> path;
	/*
		current direction enemy is facing
	*/
	Enums.directions direction;
	/*
		enemy name
	*/
	string enemyName;
	/*
		last direction that the enemy moved in
		used to determine if enemy changed directions
	*/
	Vector3 lastMoveDir;

	/*
		reference to EnemyManager component
	*/
	EnemyManager manager;
	/*
		reference to NavMeshAgent component
	*/
	UnityEngine.AI.NavMeshAgent nav;


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
		//transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);
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

	/*
		Called every frame
		returns enemy to patrol if they are not alerted or distracted
		if showDebug == true, show path enemy is on
	*/
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

			if (showDebug) {
				foreach (int i in path) {
					Vector3 pos = manager.Graph.vertices [i].position;
					Debug.DrawLine (pos, pos + Vector3.up, Color.red, 0.01f);
				}
			}
		} 
	}

	/*
		Sets new destination patrol vertex if enemy has reached current patrol
	*/
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
				PauseMovement ();
			}
		}
	}

	/*
		Allows enemy to travel between vertices in graph along a certain path
		recalculates new destination as enemy approaches current destination
		sets appropriate occupant of current destination vertex and clears occupation of last occupied vertex
		if path.Count >= 2, turn in direction of next vertex in path if it is in a different direction
		update currVertex according to direction moved in
	*/
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
							Turn (moveDir);
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
						Turn (moveDir);
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

	/*
		sets path to next patrol point
	*/
	public void BackToPatrol() {
		if (patrolVertices.Count > 0) {
			List<int> newPath = manager.Graph.FindShortestPath (currVertexIndex, patrolVertices [destPatrolIndex]);
			if (newPath.Count > 0) {
				path = newPath;
			}
		}
	}

	/*
		Pauses enemy movement
		if they are alerted or distracted during this time, coroutine ends
	*/
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

	//TODO make magic numbers variables
	/*
		turns enemy in direction
		@param dir - direction enemy should turn in
	*/
	public void Turn(Vector3 dir) {
		StopCoroutine ("TurnTowards");
		StartCoroutine ("TurnTowards", dir);
	}

	IEnumerator TurnTowards(Vector3 towards) {
		if (transform.forward.normalized != towards.normalized) {
			int count = 0;
			float angle = Vector3.SignedAngle (transform.forward.normalized, towards.normalized, Vector3.up);
			while (Mathf.Abs (Vector3.Angle (transform.forward.normalized, towards.normalized)) >= 15.0f && count <= 6) {
				if (angle < 0.0f) {
					transform.rotation *= Quaternion.Euler (0.0f, -30.0f, 0.0f);
				} else {
					transform.rotation *= Quaternion.Euler (0.0f, 30.0f, 0.0f);
				}
				count++;
				yield return null;
			}
			transform.rotation = Quaternion.LookRotation (towards);
		}
	}
}
