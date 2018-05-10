using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
	Class to detect swipes and move player accordingly
*/
public class SwipeManager : MonoBehaviour {

	/*
		reference to PlayerMovement component
	*/
	PlayerMovement playerMovement;
	/*
		direction player will move in
	*/
	Vector2 moveDir;
	/*
		location of touch start on screen
	*/
	Vector2 touchStart;
	/*
		location of touch end on screen
	*/
	Vector2 touchEnd;

	void Start () {
		moveDir = Vector2.zero;
		playerMovement = GetComponent<PlayerMovement> ();
	}

	/*
		Called every frame
		detects touches and swipes and moves player based on swipe direction
	*/
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
				/*case TouchPhase.Ended:
					moveDir = Vector2.zero;
					playerMovement.StopMoving ();
					break;*/
				}
			}
		}
	}

	/*
		moves player in direction of swipe based on whether x or y component  of dir is larger
		@param dir - direction of swipe
	*/
	void MoveInDirection(Vector2 dir) {
		if (moveDir.magnitude > 5.0f && playerMovement) {
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
		}
	}
}
