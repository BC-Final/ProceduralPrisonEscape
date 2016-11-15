using UnityEngine;
using System.Collections;

public class TestNodePacketSend : MonoBehaviour {
	[SerializeField]
	private float _interval;

	private float timer;

	private void Update() {
		if (timer - Time.time <= 0) {
			int startIndex = Random.Range(0, AbstractNode.Graph.Count);
			int endIndex = Random.Range(0, AbstractNode.Graph.Count);

			if (startIndex != endIndex) {
				GameObject go = (Instantiate(Resources.Load("Packets/BasePacket"), AbstractNode.Graph[startIndex].transform.position, Quaternion.identity, transform) as GameObject);
				go.GetComponent<Packet>().Send(AbstractNode.Graph[startIndex], AbstractNode.Graph[endIndex]);
			}

			timer = _interval + Time.time;
		}
	}
}
