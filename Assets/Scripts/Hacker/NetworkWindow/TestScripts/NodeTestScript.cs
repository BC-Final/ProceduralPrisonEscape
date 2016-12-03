using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class NodeTestScript : MonoBehaviour {
	[SerializeField]
	private bool _active;

	[SerializeField]
	private AbstractNode _start;
	
	public void OnClick(AbstractNode pNode) {
		if (_active) {
			if (pNode == _start) {
				_start = null;
				pNode.GetComponent<Image>().color = Color.white;
			} else if (_start == null) {
				_start = pNode;
				pNode.GetComponent<Image>().color = Color.green;
			} else {
				GameObject go = (Instantiate(Resources.Load("Packets/BasePacket"), _start.transform.position, Quaternion.identity, transform) as GameObject);
				go.GetComponent<Packet>().Send(_start, pNode);
			}
		}
	}
}
