using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeLink : MonoBehaviour {

    [SerializeField]
    GameObject firstSegment;
    [SerializeField]
    GameObject secondSegment;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Vector3 toNext = firstSegment.transform.position - secondSegment.transform.position;
        Vector3 newForward = Vector3.RotateTowards(transform.forward, toNext, 1.0f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newForward);
        transform.position = (firstSegment.transform.position + secondSegment.transform.position) / 2.0f;

    }
}
