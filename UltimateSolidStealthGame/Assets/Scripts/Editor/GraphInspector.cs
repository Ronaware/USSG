using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GraphInspector : EditorWindow {

    int buttonIndex = 0;
    Graph graph;
    int clickedVertIndex;
    Vector3 clickedVertPos;
    string clickedVertOccuppiedBy;
    string clickedVertChildVerts;
    int clickedVertParentVert;

    [MenuItem("Window/Graph Visualization")]
	static void Init() {
        GraphInspector gi = (GraphInspector)EditorWindow.GetWindow(typeof(GraphInspector));
        gi.Show();
    }

    public void OnInspectorUpdate() {
        Repaint();
    }

    void OnGUI() {
        GUILayout.BeginHorizontal();
        SetButtons();
        GUI.color = Color.white;
        GUILayout.BeginVertical("Box");
        clickedVertIndex = EditorGUILayout.IntField("Index", clickedVertIndex);
        clickedVertPos = EditorGUILayout.Vector3Field("Position", clickedVertPos);
        clickedVertOccuppiedBy = EditorGUILayout.TextField("Occupied By", clickedVertOccuppiedBy);
        clickedVertParentVert = EditorGUILayout.IntField("Parent Vertex", clickedVertParentVert);
        EditorGUILayout.PrefixLabel("Children Vertices");
        clickedVertChildVerts = EditorGUILayout.TextArea(clickedVertChildVerts);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    void SetButtons() {
        Transform[] transforms = Selection.transforms;
        if (transforms.Length == 1) {
            graph = transforms[0].gameObject.GetComponent<Graph>();
            if (graph) {
                int graphArea = graph.GridHeight * graph.GridWidth;
                if (graphArea > 0) {
                    GUILayout.BeginVertical();
                    for (int i = graphArea - graph.GridWidth; i >= 0; i -= graph.GridWidth) {
                        GUILayout.BeginHorizontal();
                        for (int j = 0; j < graph.GridWidth; j++) {
                            if (graph.vertices[i + j] != null) {
                                if (!graph.vertices[i + j].occupied) {
                                    GUI.color = Color.green;
                                } else {
                                    GUI.color = Color.red;
                                }
                            }
                            else {
                                GUI.color = Color.gray;
                            }
                            if (GUILayout.Button(" ", GUILayout.Width(7.0f), GUILayout.Height(7.0f))) {
                                DisplayVertexData(i + j);
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                }
            }
        }
    }

    void DisplayVertexData(int index) {
        if (graph) {
            int graphArea = graph.GridHeight * graph.GridWidth;
            if (graphArea > 0) {
                Vertex currVert = graph.vertices[index];
                if (currVert != null) {
                    GUI.color = Color.white;
                    clickedVertIndex = currVert.index;
                    clickedVertPos = currVert.position;
                    clickedVertOccuppiedBy = currVert.occupiedBy;
                    int tempParent = (currVert.parentVertex != null) ? currVert.parentVertex.index : -1;
                    clickedVertParentVert = tempParent;
                    string temp = "";
                    foreach (Vertex child in currVert.childVertices) {
                        temp += (child.index.ToString() + "\n");
                    }
                    clickedVertChildVerts = temp;
                }
            }
        }
    }
}


