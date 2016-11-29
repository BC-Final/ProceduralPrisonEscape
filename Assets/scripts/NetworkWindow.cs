using UnityEngine;
using System.Collections;

public class NetworkWindow : MonoBehaviour {
	private NetworkWindow _instance;

	public NetworkWindow Instance {
		get {
			if (_instance == null) {
				Debug.LogError("NetworkWindow missing!");
			}

			return _instance;
		}
	}

	public void CreateNode (NodeType pType) {
		string loadString = "";

		switch (pType) {
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
				loadString = "pdb_DoorNode";
				break;
			case NodeType.Firewall:
				loadString = "pdb_FirewallNode";
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

		Resources.Load("prefabs/hacker/nodes" + loadString);
	}
}
