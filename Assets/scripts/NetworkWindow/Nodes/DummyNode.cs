using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SelectionBase]
public class DummyNode : MonoBehaviour {
	private enum NodeType {
		Base,
		Door,
		Firewall,
		Dispenser,
		Security,
		Hacker,
		Database
	}

	private int _id;

	[SerializeField]
	private NodeType _type;

	[SerializeField]
	private GameObject _associatedObject;
	//TODO Send information after init

	[SerializeField]
	private List<DummyNode> _connections;

	public void Initialize () {

	}

	public List<DummyNode> GetConnections () {
		return new List<DummyNode>(_connections);
	}

	public void AddConnection (DummyNode pNode) {
		if (!_connections.Contains(pNode)) {
			_connections.Add(pNode);
		}
	}

	public void RemoveConnection (DummyNode pNode) {
		if (_connections.Contains(pNode)) {
			_connections.Remove(pNode);
		}
	}

	private void OnDrawGizmos () {
		if (_connections != null) {
			foreach (DummyNode n in _connections) {
				if (n != null) {
					if (n.GetConnections().FindAll(x => x == this).Count == 0) {
						Gizmos.color = Color.red;
					} else if (GetConnections().FindAll(x => x == n).Count > 1) {
						Gizmos.color = Color.blue;
					} else {
						Gizmos.color = Color.white;
					}

					Gizmos.DrawLine(transform.position, transform.position + (n.transform.position - transform.position) / 2.0f);
				}
			}
		}
	}
}
