using UnityEngine;
using System.Collections;

public class HackerNode : AbstractNode {
	protected override void Awake() {
		base.Awake();
		Instantiate(Resources.Load("Avatars/HackerAvatar"), transform.position, Quaternion.identity, transform.parent);
	}
}
