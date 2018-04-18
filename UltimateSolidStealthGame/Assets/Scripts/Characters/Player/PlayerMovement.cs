using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField]
	int currVertexIndex;

	int lastVertexIndex;
	Graph graph;
	float moveAmount;
	Vector3 movement;
	UnityEngine.AI.NavMeshAgent nav;
	Enums.directions direction;
	string playerName;

	public int CurrVertexIndex {
		get {
			return currVertexIndex;
		}
	}

	// Use this for initialization
	void Start () {
		movement = Vector3.zero;
		GameObject temp = GameObject.FindGameObjectWithTag ("Graph");
		if (temp) {
			graph = temp.GetComponent<Graph> ();
			if (graph) {
				moveAmount = graph.VertexDistance;
				nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
				transform.position.Set(transform.position.x, 0.0f, transform.position.z);
				currVertexIndex = graph.GetIndexFromPosition(transform.position);
				lastVertexIndex = currVertexIndex;
				playerName = gameObject.name;
				graph.vertices [currVertexIndex].occupiedBy = playerName;
				graph.vertices [currVertexIndex].occupied = true;
			}
		}
	}

	void Update() {  
		if (graph.Ready == true) {
			SetNewDestination ();
		}
	}

	void SetNewDestination() {
		if (nav != null && graph != null) {
			if (nav.remainingDistance <= 0.1f) {
				if (lastVertexIndex != currVertexIndex) {
					if (graph.vertices[lastVertexIndex].occupiedBy == playerName) {
						graph.vertices [lastVertexIndex].occupied = false;
						graph.vertices [lastVertexIndex].occupiedBy = "";
					}
					graph.vertices [currVertexIndex].occupied = true;
					graph.vertices [currVertexIndex].occupiedBy = playerName;
				}
				if (movement != Vector3.zero) {
					switch (direction) {
					case Enums.directions.left:
						if (graph.vertices [currVertexIndex - 1] != null) {
							if (graph.vertices [currVertexIndex - 1].occupied == true) {
								StopMoving ();
								return;
							}
							lastVertexIndex = currVertexIndex;
							currVertexIndex -= 1;
						} else {
							StopMoving ();
						}
						break;
					case Enums.directions.right:
						if (graph.vertices [currVertexIndex + 1] != null) {
							if (graph.vertices [currVertexIndex + 1].occupied == true) {
								StopMoving ();
								return;
							}
							lastVertexIndex = currVertexIndex;
							currVertexIndex += 1;
						} else {
							StopMoving ();
						}
						break;
					case Enums.directions.up:
						if (graph.vertices [currVertexIndex + graph.GridWidth] != null) {
							if (graph.vertices [currVertexIndex + graph.GridWidth].occupied == true) {
								StopMoving ();
								return;
							}
							lastVertexIndex = currVertexIndex;
							currVertexIndex += graph.GridWidth;
						} else {
							StopMoving ();
						}
						break;
					case Enums.directions.down:
						if (graph.vertices [currVertexIndex - graph.GridWidth] != null) {
							if (graph.vertices [currVertexIndex - graph.GridWidth].occupied == true) {
								StopMoving ();
								return;
							}
							lastVertexIndex = currVertexIndex;
							currVertexIndex -= graph.GridWidth;

						} else {
							StopMoving ();
						}
						break;
					}
					graph.vertices [lastVertexIndex].occupied = true;
					graph.vertices [lastVertexIndex].occupiedBy = playerName;
					graph.vertices [currVertexIndex].occupied = true;
					graph.vertices [currVertexIndex].occupiedBy = playerName;
					nav.SetDestination (graph.vertices [currVertexIndex].position);
				} 
			}
		}
	}

	public void MoveUntilStop(Enums.directions dir) {
		direction = dir;
		switch (dir) {
		case Enums.directions.left:
			movement.Set (-moveAmount, 0.0f, 0.0f);
			break;
		case Enums.directions.right:
			movement.Set (moveAmount, 0.0f, 0.0f);
			break;
		case Enums.directions.up:
			movement.Set (0.0f, 0.0f, moveAmount);
			break;
        case Enums.directions.down:
            movement.Set(0.0f, 0.0f, -moveAmount);
			break;
		}
		transform.rotation = Quaternion.LookRotation(movement);
	}

	public void StopMoving() {
		movement = Vector3.zero;
	}
}
