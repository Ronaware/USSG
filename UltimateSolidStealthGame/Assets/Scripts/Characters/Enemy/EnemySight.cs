using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {

	[SerializeField]
	int sightDistance = 6;
	[SerializeField]
	int fov = 30;
	[SerializeField]
	int numFramesToResetPath = 1;

	int frames;
	PlayerMovement playerMovement;
	Graph graph;
	EnemyMovement movement;
	int ignoreEnemiesLayer;

	public int IgnoreEnemiesLayer {
		get { return ignoreEnemiesLayer; }
	}
		
	// Use this for initialization
	void Start () {
		frames = 0;
		ignoreEnemiesLayer = 1 << LayerMask.NameToLayer ("Enemy");
		ignoreEnemiesLayer = ~ignoreEnemiesLayer;
		GameObject temp = GameObject.FindGameObjectWithTag ("Player");
		if (temp) {
			playerMovement = temp.GetComponent<PlayerMovement> ();
		}
		temp = GameObject.FindGameObjectWithTag ("Graph");
		if (temp) {
			graph = temp.GetComponent<Graph> ();
		}
		movement = gameObject.GetComponent<EnemyMovement> ();
	}
	
	// Update is called once per frame
	void Update () {
		frames = (frames + 1) % numFramesToResetPath;
		if (playerMovement != null && movement != null) {
			if (frames == (numFramesToResetPath - 1)) {
				Vector3 toPlayer = playerMovement.transform.position - gameObject.transform.position;
				Vector3 front = gameObject.transform.forward;
				float angle = Vector3.Angle (front.normalized, toPlayer.normalized);
				if (angle <= fov && toPlayer.magnitude <= sightDistance) {
					RaycastHit hit;
					if (Physics.Raycast(transform.position, toPlayer, out hit, Mathf.Infinity, ignoreEnemiesLayer)) {
						if (hit.transform.CompareTag("Player") == true) {
							List<int> pathToPlayer = graph.FindShortestPath (movement.CurrVertexIndex, playerMovement.CurrVertexIndex);
							if (pathToPlayer != null) {
								
								foreach (int i in pathToPlayer) {
									Debug.DrawLine (graph.vertices [i].position, graph.vertices [i].position + Vector3.up, Color.red, 0.5f);
								}

								movement.Alerted = true;
								movement.Path = pathToPlayer;
							}
						}
					}
				}
			}
		}
	}
}
