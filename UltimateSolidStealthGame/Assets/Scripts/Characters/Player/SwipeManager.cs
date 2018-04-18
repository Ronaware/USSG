using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : MonoBehaviour {

	PlayerMovement playerMovement;
	Vector2 moveDir;
	Vector2 touchStart;
	Vector2 touchEnd;

	// Use this for initialization
	void Start () {
		moveDir = Vector2.zero;
		playerMovement = GetComponent<PlayerMovement> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			int id = Input.GetTouch (0).fingerId;
			if (!EventSystem.current.IsPointerOverGameObject (id)) {
				Touch touch = Input.GetTouch (0);
				switch (touch.phase) {
				case TouchPhase.Began:
					playerMovement.StopMoving ();
					touchStart = touch.position;
					moveDir = Vector2.zero;
					break;
				case TouchPhase.Moved:
					moveDir = touch.position - touchStart;
					touchStart = touch.position;
					MoveInDirection (moveDir);
					break;
				}
			}
		}
	}

	void MoveInDirection(Vector2 dir) {
		if (moveDir.magnitude > 6.0f && playerMovement) {
			float xDir = Mathf.Abs (dir.x);
			float yDir = Mathf.Abs (dir.y);
			if (xDir >= yDir) {
				if (moveDir.x > 0) {
					playerMovement.MoveUntilStop (Enums.directions.right);
				} else if (moveDir.x < 0) {
					playerMovement.MoveUntilStop (Enums.directions.left);
				}
			} else if (xDir < yDir) {
				if (moveDir.y > 0) {
					playerMovement.MoveUntilStop (Enums.directions.up);
				} else if (moveDir.y < 0) {
					playerMovement.MoveUntilStop (Enums.directions.down);
				}
			}
		} else {
			playerMovement.StopMoving ();
		}
	}
}
