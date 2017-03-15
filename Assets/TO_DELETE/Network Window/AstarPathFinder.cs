//using UnityEngine;
//using System.Collections.Generic;
//using System.Linq;

//public class AstarPathFinder {
//	/// <summary>
//	/// Calculates the shortest path from pStart to pEnd.
//	/// </summary>
//	/// <param name="pStart">The start node</param>
//	/// <param name="pEnd">The target node</param>
//	/// <returns>Returns a the path as lists of nodes, with start beeing at index 0</returns>
//	public static List<AbstractNode> CalculatePath(AbstractNode pStart, AbstractNode pEnd) {
//		List<AbstractNode> closed = new List<AbstractNode>();
//		List<AbstractNode> open = new List<AbstractNode>();
//		Dictionary<AbstractNode, AbstractNode> cameFrom = new Dictionary<AbstractNode, AbstractNode>();
//		Dictionary<AbstractNode, float> gScore = new Dictionary<AbstractNode, float>();
//		Dictionary<AbstractNode, float> fScore = new Dictionary<AbstractNode, float>();

//		open.Add(pStart);
//		gScore[pStart] = 0.0f;
//		fScore[pStart] = Vector3.Distance(pStart.transform.position, pEnd.transform.position);

//		while (open.Count != 0) {
//			AbstractNode current = getNearestNode(open, fScore);

//			if (current.gameObject == pEnd.gameObject) {
//				List<AbstractNode> path = new List<AbstractNode>();
//				path.Add(current);

//				while (cameFrom.ContainsKey(current)) {
//					current = cameFrom[current];
//					path.Add(current);
//				}

//				path.Reverse();

//				return path;
//			}

//			open.Remove(current);
//			closed.Add(current);

//			foreach (AbstractNode neighbor in current.GetConnections()) {
//				if (closed.Contains(neighbor)) {
//					continue;
//				}

//				float tentative_gScore = gScore[current] + Vector3.Distance(current.transform.position, neighbor.transform.position);

//				if (!open.Contains(neighbor)) {
//					open.Add(neighbor);
//				} else if (tentative_gScore >= gScore[neighbor]) {
//					continue;
//				}

//				cameFrom[neighbor] = current;
//				gScore[neighbor] = tentative_gScore;
//				fScore[neighbor] = gScore[neighbor] + Vector3.Distance(neighbor.transform.position, pEnd.transform.position);
//			}
//		}

//		return null;
//	}

//	public static bool PathExists (AbstractNode pStart, AbstractNode pEnd) {
//		List<AbstractNode> closed = new List<AbstractNode>();
//		List<AbstractNode> open = new List<AbstractNode>();
//		Dictionary<AbstractNode, AbstractNode> cameFrom = new Dictionary<AbstractNode, AbstractNode>();
//		Dictionary<AbstractNode, float> gScore = new Dictionary<AbstractNode, float>();
//		Dictionary<AbstractNode, float> fScore = new Dictionary<AbstractNode, float>();

//		open.Add(pStart);
//		gScore[pStart] = 0.0f;
//		fScore[pStart] = Vector3.Distance(pStart.transform.position, pEnd.transform.position);

//		while (open.Count != 0) {
//			AbstractNode current = getNearestNode(open, fScore);

//			if (current.gameObject == pEnd.gameObject) {
//				return true;
//			}

//			open.Remove(current);
//			closed.Add(current);

//			foreach (AbstractNode neighbor in current.GetConnections()) {
//				if (closed.Contains(neighbor)) {
//					continue;
//				}

//				if ((neighbor is FirewallNode) && !(neighbor as FirewallNode).AssociatedFirewall.Destroyed.Value) {
//					continue;
//				}

//				float tentative_gScore = gScore[current] + Vector3.Distance(current.transform.position, neighbor.transform.position);

//				if (!open.Contains(neighbor)) {
//					open.Add(neighbor);
//				} else if (tentative_gScore >= gScore[neighbor]) {
//					continue;
//				}

//				cameFrom[neighbor] = current;
//				gScore[neighbor] = tentative_gScore;
//				fScore[neighbor] = gScore[neighbor] + Vector3.Distance(neighbor.transform.position, pEnd.transform.position);
//			}
//		}

//		return false;
//	} 

//	private static AbstractNode getNearestNode(List<AbstractNode> pOpenNodes, Dictionary<AbstractNode, float> pCost) {
//		Dictionary<AbstractNode, float> unusedNodes = new Dictionary<AbstractNode, float>();

//		foreach (AbstractNode n in pOpenNodes) {
//			unusedNodes.Add(n, pCost[n]);
//		}

//		return unusedNodes.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
//	}
//}
