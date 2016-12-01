using UnityEngine;
using System.Collections;

public class HackerNode : AbstractNode {
	protected override void Awake() {
		base.Awake();
	}

	public void CreateAvatar () {
		Instantiate(Resources.Load("prefabs/hacker/avatars/HackerAvatar"), transform.position, Quaternion.identity, transform.parent);
	}
}
