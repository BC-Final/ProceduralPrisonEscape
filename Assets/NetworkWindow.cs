using UnityEngine;
using System.Collections;

public class NetworkWindow : MonoBehaviour {
	public void CreateNode (NodeType pType) {
		string loadString = "";

		switch (pType) {
			case NodeType.Base:
				loadString = "pfb_BaseNode";
				break;
			case NodeType.Database:
				loadString = "pfb_BaseNode";
				break;
			case NodeType.Dispenser:
				loadString = "pfb_BaseNode";
				break;
			case NodeType.Door:
				loadString = "pfb_BaseNode";
				break;
			case NodeType.Firewall:
				loadString = "pfb_BaseNode";
				break;
			case NodeType.Hacker:
				loadString = "pfb_BaseNode";
				break;
			case NodeType.Security:
				loadString = "pfb_BaseNode";
				break;
		}
	}
}
