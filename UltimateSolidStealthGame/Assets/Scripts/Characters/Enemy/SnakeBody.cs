using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnakeBody : MonoBehaviour {

    [SerializeField]
    GameObject graphObject;
    [SerializeField]
    GameObject nextSegment;

    SnakeBody nextSegmentBody;
    Graph graph;
    UnityEngine.AI.NavMeshAgent nav;
    int currIndex;
    int lastIndex;

    void Start () {
        if (nextSegment) {
            nextSegmentBody = nextSegment.GetComponent<SnakeBody>();
        }
        graph = graphObject.GetComponent<Graph>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        currIndex = graph.GetIndexFromPosition(transform.position);
        lastIndex = currIndex;
	}
	
	void Update () {
	    if (!nextSegmentBody) {
            if (nav.remainingDistance <= 1.0f) {
                graph.vertices[lastIndex].occupied = false;
                graph.vertices[lastIndex].occupiedBy = "";
                graph.vertices[lastIndex].NotifyParentOrChild();
            }
        }
	}

    public void SetDestination(int destIndex) {
        lastIndex = currIndex;
        currIndex = destIndex;
        nav.SetDestination(graph.vertices[destIndex].position);
        graph.vertices[currIndex].occupied = true;
        graph.vertices[currIndex].occupiedBy = "Enemy";
        graph.vertices[currIndex].NotifyParentOrChild();
        if (nextSegmentBody) {
            nextSegmentBody.SetDestination(lastIndex);
        }
    }
}
