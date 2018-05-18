using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGraph : Graph {

    [SerializeField]
    GameObject childGraphObject;
    [SerializeField]
    int bottomLeftMapping = -1;

    Graph childGraph;

    public override void Awake() {
        base.Awake();
        childGraph = childGraphObject.GetComponent<Graph>();
        SetChildrenAndParent();
    }

    void SetChildrenAndParent() {
        int ratio = (int)(vertexDistance / childGraph.VertexDistance);
        int currParentIndex = 0;
        bottomLeftMapping = GetBottomLeftMapping();
        if (bottomLeftMapping != -1) {
            for (int i = 0; i < gridHeight; i++) {
                for (int j = 0; j < gridWidth; j++) {
                    for (int k = 0; k < ratio; k++) {
                        for (int l = 0; l < ratio; l++) {
                            int currChildIndex = bottomLeftMapping + (j * ratio) + (i * ratio * childGraph.GridWidth) + l + (k * childGraph.GridWidth);
                            vertices[currParentIndex].childVertices.Add(childGraph.vertices[currChildIndex]);
                            Vertex temp = childGraph.vertices[currChildIndex];
                            if (temp != null) {
                                temp.parentVertex = vertices[currParentIndex];
                            }
                        }
                    }
                    currParentIndex++;
                }
            }
        }
    }

    int GetBottomLeftMapping() {
        Vector3 bottomLeft = box.center;
        bottomLeft[0] -= width / 2.0f;
        bottomLeft[0] += childGraph.VertexDistance / 2.0f;
        bottomLeft[2] -= height / 2.0f;
        bottomLeft[2] += childGraph.VertexDistance / 2.0f;
        bottomLeft = transform.TransformPoint(bottomLeft);
        return childGraph.GetIndexFromPosition(bottomLeft);
    }

    void Update() {

    }
}
