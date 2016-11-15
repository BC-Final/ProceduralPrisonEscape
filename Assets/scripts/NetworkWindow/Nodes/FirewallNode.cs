using UnityEngine;
using System.Collections;

public class FirewallNode : AbstractNode {
	[SerializeField]
	private float _feedbackPacketInterval;

	private bool _broken;

	protected override void GotHacked() {

	}
}
