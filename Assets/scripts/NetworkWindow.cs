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
			string loadString = "";

			switch ((NodeType)nc.NodeType) {
				case NodeType.Base:
					loadString = "pfb_BaseNode";
					break;
				case NodeType.Database:
					loadString = "pfb_DatabaseNode";
					break;
				case NodeType.Dispenser:
					loadString = "pfb_DispenserNode";
					break;
				case NodeType.Door:
					loadString = "pfb_DoorNode";
					break;
				case NodeType.Firewall:
					loadString = "pfb_FirewallNode";
					break;
				case NodeType.Hacker:
					loadString = "pfb_HackerNode";
					break;
				case NodeType.Security:
					loadString = "pfb_SecurityNode";
					break;
				default:
					Debug.LogError("Node Type Not known!");
					break;
			}

			Object o = Resources.Load("prefabs/hacker/nodes/" + loadString);
			GameObject go = Instantiate(o, GetComponentInChildren<Canvas>().transform) as GameObject;

			if (go == null)
				Debug.Log("Cant find " + loadString);

			AbstractNode an = go.GetComponent<AbstractNode>();

			an.Id = nc.ID;
			_nodes.Add(an);

			RectTransform rt = go.GetComponent<RectTransform>();
			rt.anchoredPosition = new Vector2(nc.X, nc.Y);
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
