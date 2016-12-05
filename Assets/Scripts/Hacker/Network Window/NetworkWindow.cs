using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkWindow : MonoBehaviour {
	private static NetworkWindow _instance;

	public static NetworkWindow Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<NetworkWindow>();

				if (_instance == null) {
					Debug.LogError("NetworkWindow missing!");
				}
			}

			return _instance;
		}
	}

	private List<AbstractNode> _nodes = new List<AbstractNode>();
	private List<CustomCommands.Creation.NodeCreation> _nodeData = new List<CustomCommands.Creation.NodeCreation>();

	private AbstractNode GetNodeById (int pId) {
		return _nodes.Find(x => x.Id == pId);
	}

	public void AddNode (CustomCommands.Creation.NodeCreation pPackage) {
		_nodeData.Add(pPackage);
	}

	public void FinishedReceivingAll () {
		foreach (CustomCommands.Creation.NodeCreation nc in _nodeData) {
			GameObject prefab = null;

			switch ((NodeType)nc.NodeType) {
				case NodeType.Base:
					prefab = HackerReferenceManager.Instance.BaseNode;
					break;
				case NodeType.Database:
					prefab = HackerReferenceManager.Instance.DatabaseNode;
					break;
				case NodeType.Dispenser:
					prefab = HackerReferenceManager.Instance.DispenserNode;
					break;
				case NodeType.Door:
					prefab = HackerReferenceManager.Instance.DoorNode;
					break;
				case NodeType.Firewall:
					prefab = HackerReferenceManager.Instance.FirewallNode;
					break;
				case NodeType.Hacker:
					prefab = HackerReferenceManager.Instance.HackerNode;
					break;
				case NodeType.Security:
					prefab = HackerReferenceManager.Instance.SecurityNode;
					break;
				default:
					Debug.LogError("Node Type Not known!");
					break;
			}

			GameObject go = Instantiate(prefab, GetComponentInChildren<Canvas>().transform) as GameObject;
			AbstractNode an = go.GetComponent<AbstractNode>();

			an.Id = nc.ID;
			_nodes.Add(an);

			RectTransform rt = go.GetComponent<RectTransform>();
			rt.anchoredPosition3D = new Vector3(nc.X, nc.Y, 0.0f);
		}

		foreach (CustomCommands.Creation.NodeCreation nc in _nodeData) {
			AbstractNode n = GetNodeById(nc.ID);
		
			foreach (int id in nc.ConnectionIds) {
				n.AddConnection(GetNodeById(id));
			}
		}

		_nodeData.ForEach(x => GetNodeById(x.ID).SetReferences(x.AsocID));
		(_nodes.Find(x => x is HackerNode) as HackerNode).CreateAvatar();
		_nodes.FindAll(x => x is SecurityNode).ForEach(x => (x as SecurityNode).CreateAvatar());

		_nodeData.Clear();
		_nodeData = null;
	}


	public void RecalculateAccesibleNodes() {
		foreach (AbstractNode n in _nodes) {
			n.SetAccessible(AstarPathFinder.PathExists(FindObjectOfType<HackerAvatar>().CurrentNode, n));
		}
	}
}
