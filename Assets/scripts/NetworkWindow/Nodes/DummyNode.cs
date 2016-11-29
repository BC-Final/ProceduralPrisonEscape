using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[SelectionBase]
public class DummyNode : MonoBehaviour, INetworked {
	private static List<DummyNode> _nodes = new List<DummyNode>();
	public static List<DummyNode> GetNodeList () { return _nodes; }

	private int _id;

	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}

	[SerializeField]
	private NodeType _type;
	public NodeType Type { get { return _type; } }

	[SerializeField]
	private GameObject _associatedObject;
	public GameObject AssociatedObject { get { return _associatedObject; } }

	[SerializeField]
	private List<DummyNode> _connections;

	public void Initialize () {
		
	}

	private void Awake () {
		_nodes.Add(this);
	}

	private void OnDestroy () {
		_nodes.Remove(this);
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
