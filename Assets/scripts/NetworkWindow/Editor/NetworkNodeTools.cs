using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NetworkNodeTools : MonoBehaviour {
	[MenuItem("Node Tools/Connect Selected Nodes %g")]
	static void ConnectSelectedNodes() {
		List<AbstractNode> selectedNodes = new List<AbstractNode>();

		
		foreach (GameObject n in Selection.gameObjects) {
			if (n.GetComponent<AbstractNode>() != null) {
				selectedNodes.Add(n.GetComponent<AbstractNode>());
			}
		}

		if (selectedNodes.Count == 2) {
			foreach (AbstractNode n in selectedNodes) {
				foreach (AbstractNode c in selectedNodes) {
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

	[MenuItem("Node Tools/Deconnect Selected Nodes %h")]
	static void DeconnectSelectedNodes() {
		List<AbstractNode> selectedNodes = new List<AbstractNode>();


		foreach (GameObject n in Selection.gameObjects) {
			if (n.GetComponent<AbstractNode>() != null) {
				selectedNodes.Add(n.GetComponent<AbstractNode>());
			}
		}

		if (selectedNodes.Count == 2) {
			foreach (AbstractNode n in selectedNodes) {
				foreach (AbstractNode c in selectedNodes) {
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
}