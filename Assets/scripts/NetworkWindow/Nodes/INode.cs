using UnityEngine;
using System.Collections;

public interface INode {
	GameObject gameObject { get; }
	void AddConnection (INode pNode);
	void RemoveConnection (INode pNode);
}
