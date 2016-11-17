using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TestNodeConnect : MonoBehaviour {
	[SerializeField]
	private int _testConnectCount;

	[SerializeField]
	private float _testConnectDistance;

	private void Start() {
		foreach (AbstractNode node in AbstractNode.Graph) {
			Dictionary<AbstractNode, float> distance = new Dictionary<AbstractNode, float>();

			foreach (AbstractNode neighbor in AbstractNode.Graph) {
				if (node.gameObject == neighbor.gameObject) {
					continue;
				}

				distance[neighbor] = Vector3.Distance(node.transform.position, neighbor.transform.position);
			}

			IOrderedEnumerable<KeyValuePair<AbstractNode, float>> sortedDistance = distance.OrderBy(x => x.Value);

			int counter = 0;

			while (node.GetConnections().Count < _testConnectCount && counter < sortedDistance.Count()) {
				AbstractNode neighbor = sortedDistance.ElementAt(counter).Key;
				if (neighbor.GetConnections().Count < _testConnectCount && Vector3.Distance(node.transform.position, neighbor.transform.position) < _testConnectDistance) {
					node.AddConnection(neighbor);
					neighbor.AddConnection(node);
				}

				counter++;
			}
		}
	}
}
