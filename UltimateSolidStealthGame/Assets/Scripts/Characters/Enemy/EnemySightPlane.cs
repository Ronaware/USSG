using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class used to alter shape of mesh into one that reflects enemy's sight cone
*/
public class EnemySightPlane : MonoBehaviour {

	/*
		prefab of plane mesh
	*/
	[SerializeField]
	GameObject planePrefab;

	/*
		instance of planePrefab
	*/
	GameObject sightPlane;
	/*
		mesh component of sightPlane
	*/
	Mesh sightPlaneMesh;
	/*
		layer defining all items in "Default" layer (Walls)
	*/
	int defaultLayer;
	/*
		reference to EnemyManager component
	*/
	EnemyManager manager;
	/*
		vertices that define shape of new mesh
	*/
	List<Vector3> newVerts;
	/*
		indices of triangles in new mesh shape
	*/
	List<int> newIndices;

	void Start () {
		manager = GetComponent<EnemyManager> ();
		newVerts = new List<Vector3> ();
		newIndices = new List<int> ();
		defaultLayer = 1 << LayerMask.NameToLayer ("Default");
		sightPlane = Instantiate (planePrefab);
		sightPlane.transform.position = Vector3.zero;
		sightPlaneMesh = sightPlane.GetComponent<MeshFilter> ().mesh;
		sightPlaneMesh.Clear ();
	}

	void Update () {
		CreatePlane ();
	}

	/*
		Raycasts at certain angles in enemy's line of sight
		points of ray intersection determine vertices of sight mesh shape
		goes from -fov to fov in equal angular increments
	*/
	void CreatePlane() {
		sightPlaneMesh.Clear ();
		newIndices.Clear ();
		newVerts.Clear ();
		newVerts.Add (transform.position);
		int fov = manager.Sight.CurrentFOV;
		float currAngle = -fov;
		float deltaAngle = (fov * 2.0f) / (fov - 1);
		for (int i = 0; i < fov; i++) {
			Vector3 dir = Quaternion.AngleAxis (currAngle, Vector3.up) * transform.forward;
			RaycastHit hit;
			if (Physics.Raycast (transform.position, dir, out hit, manager.Sight.SightDistance, defaultLayer)) {
				newVerts.Add (hit.point);
			} else {
				newVerts.Add (transform.position + (manager.Sight.SightDistance * dir));
			}
			if (i == 0) {
				newIndices.Add (i);
			} else if (i != fov - 1) {
				newIndices.Add (i);
				newIndices.Add (0);
				newIndices.Add (i);
			} else {
				newIndices.Add (i);
				newIndices.Add (0);
			}
			currAngle += deltaAngle;
		}
		sightPlaneMesh.vertices = newVerts.ToArray ();
		sightPlaneMesh.triangles = newIndices.ToArray ();
	}

    void OnDisable() {
        if (sightPlane) {
            Destroy(sightPlane);
        }
    }
}
