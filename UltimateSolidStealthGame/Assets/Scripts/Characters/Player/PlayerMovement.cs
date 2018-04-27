using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField]
	int currVertexIndex;

	int lastVertexIndex;
	float moveAmount;
	Vector3 movement;
	Enums.directions direction;
	string playerName;

	PlayerManager manager;
	UnityEngine.AI.NavMeshAgent nav;

	public int CurrVertexIndex {
		get { return currVertexIndex; }
	}
	public Vector3 Movement {
		get { return movement; }
	}
	public UnityEngine.AI.NavMeshAgent Nav {
		get { return nav; }
	}
	public Enums.directions Direction {
		get { return direction; }
	}

	// Use this for initialization
	void Start () {
		movement = Vector3.zero;
		manager = GetComponent<PlayerManager> ();
		moveAmount = manager.Graph.VertexDistance;
		nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
		currVertexIndex = manager.Graph.GetIndexFromPosition(transform.position);
		lastVertexIndex = currVertexIndex;
		playerName = gameObject.name;
		manager.Graph.vertices [currVertexIndex].occupiedBy = playerName;
		manager.Graph.vertices [currVertexIndex].occupied = true;
		Vector3 forwardDir = transform.forward.normalized;
		if (forwardDir == Vector3.forward) {
			direction = Enums.directions.up;
		} else if (forwardDir == Vector3.back) {
			direction = Enums.directions.down;
		} else if (forwardDir == Vector3.left) {
			direction = Enums.directions.left;
		} else if (forwardDir == Vector3.right) {
			direction = Enums.directions.right;
		}
	}

	void Update() {  
		if (manager.Graph.Ready == true) {
			SetNewDestination ();
		}
	}

	void SetNewDestination() {
		if (nav && manager && manager.Graph) {
			if (nav.remainingDistance <= 0.1f) {
				if (lastVertexIndex != currVertexIndex) {
					if (manager.Graph.vertices[lastVertexIndex].occupiedBy == playerName) {
						manager.Graph.vertices [lastVertexIndex].occupied = false;
						manager.Graph.vertices [lastVertexIndex].occupiedBy = "";
					}
					manager.Graph.vertices [currVertexIndex].occupied = true;
					manager.Graph.vertices [currVertexIndex].occupiedBy = playerName;
				}
				if (movement != Vector3.zero) {
					transform.rotation = Quaternion.LookRotation(movement);
					switch (direction) {
					case Enums.directions.left:
						if (manager.Graph.vertices [currVertexIndex - 1] != null) {
							if (manager.Graph.vertices [currVertexIndex - 1].occupied == true) {
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
						if (manager.Graph.vertices [currVertexIndex + 1] != null) {
							if (manager.Graph.vertices [currVertexIndex + 1].occupied == true) {
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
						if (manager.Graph.vertices [currVertexIndex + manager.Graph.GridWidth] != null) {
							if (manager.Graph.vertices [currVertexIndex + manager.Graph.GridWidth].occupied == true) {
								StopMoving ();
								return;
							}
							lastVertexIndex = currVertexIndex;
							currVertexIndex += manager.Graph.GridWidth;
						} else {
							StopMoving ();
						}
						break;
					case Enums.directions.down:
						if (manager.Graph.vertices [currVertexIndex - manager.Graph.GridWidth] != null) {
							if (manager.Graph.vertices [currVertexIndex - manager.Graph.GridWidth].occupied == true) {
								StopMoving ();
								return;
							}
							lastVertexIndex = currVertexIndex;
							currVertexIndex -= manager.Graph.GridWidth;

						} else {
							StopMoving ();
						}
						break;
					}
					manager.Graph.vertices [lastVertexIndex].occupied = true;
					manager.Graph.vertices [lastVertexIndex].occupiedBy = playerName;
					manager.Graph.vertices [currVertexIndex].occupied = true;
					manager.Graph.vertices [currVertexIndex].occupiedBy = playerName;
					nav.SetDestination (manager.Graph.vertices [currVertexIndex].position);
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
	}

	public void StopMoving() {
		movement = Vector3.zero;
	}
}
