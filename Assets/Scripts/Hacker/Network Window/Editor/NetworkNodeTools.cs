using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NetworkNodeTools : MonoBehaviour {
	[MenuItem("Node Tools/Connect Selected Nodes %g")]
	static void ConnectSelectedNodes() {
		List<DummyNode> selectedNodes = new List<DummyNode>();
		
		foreach (GameObject n in Selection.gameObjects) {
			if (n.GetComponent<DummyNode>() != null) {
				selectedNodes.Add(n.GetComponent<DummyNode>());
			}
		}

		if (selectedNodes.Count == 2) {
			foreach (DummyNode n in selectedNodes) {
				foreach (DummyNode c in selectedNodes) {
					if (n.gameObject != c.gameObject) {
						Undo.RecordObject(n, "Added connection");
						n.AddConnection(c);
						EditorUtility.SetDirty(n);
					}
				}
			}
		} else {
			Debug.LogError("Wrong amount of Nodes selected: " + selectedNodes.Count + " of 2");
		}
	}

	[MenuItem("Node Tools/Disconnect Selected Nodes %h")]
	static void DisconnectSelectedNodes() {
		List<DummyNode> selectedNodes = new List<DummyNode>();

		foreach (GameObject n in Selection.gameObjects) {
			if (n.GetComponent<DummyNode>() != null) {
				selectedNodes.Add(n.GetComponent<DummyNode>());
			}
		}

		if (selectedNodes.Count == 2) {
			foreach (DummyNode n in selectedNodes) {
				foreach (DummyNode c in selectedNodes) {
					if (n.gameObject != c.gameObject) {
						Undo.RecordObject(n, "Removed connection");
						n.RemoveConnection(c);
						EditorUtility.SetDirty(n);
					}
				}
			}
		} else {
			Debug.LogError("Wrong amount of Nodes selected: " + selectedNodes.Count + " of 2");
		}
	}

	[MenuItem("Node Tools/Clear Connections %j")]
	static void ClearConnections () {
		List<DummyNode> selectedNodes = new List<DummyNode>();

		foreach (GameObject n in Selection.gameObjects) {
			if (n.GetComponent<DummyNode>() != null) {
				selectedNodes.Add(n.GetComponent<DummyNode>());
			}
		}

		foreach (DummyNode n in selectedNodes) {
			Undo.RecordObject(n, "Cleared connections");
			n.ClearConnections();
			EditorUtility.SetDirty(n);
		}
	}


	[MenuItem("Node Tools/Clear Empty Connections")]
	static void ClearEmptyConnections () {
		List<DummyNode> selectedNodes = new List<DummyNode>();

		foreach (GameObject n in Selection.gameObjects) {
			if (n.GetComponent<DummyNode>() != null) {
				selectedNodes.Add(n.GetComponent<DummyNode>());
			}
		}

		foreach (DummyNode n in selectedNodes) {
			Undo.RecordObject(n, "Cleared empty connections");
			n.ClearEmptyConnections();
			EditorUtility.SetDirty(n);
		}
	}

	[MenuItem("Node Tools/Clear Duplicate Connections")]
	static void ClearDuplicateConnections () {
		List<DummyNode> selectedNodes = new List<DummyNode>();

		foreach (GameObject n in Selection.gameObjects) {
			if (n.GetComponent<DummyNode>() != null) {
				selectedNodes.Add(n.GetComponent<DummyNode>());
			}
		}

		foreach (DummyNode n in selectedNodes) {
			Undo.RecordObject(n, "Cleared duplicate connections");
			n.ClearDuplicateConnections();
			EditorUtility.SetDirty(n);
		}
	}
}