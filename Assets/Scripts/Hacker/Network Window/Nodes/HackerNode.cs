using UnityEngine;
using System.Collections;

public class HackerNode : AbstractNode {
	protected override void Awake() {
		base.Awake();
	}

	public void CreateAvatar () {
		Instantiate(HackerReferenceManager.Instance.HackerAvatar, transform.position, Quaternion.identity, transform.parent);
	}

	public override void ToggleContext (bool pShow, HackerAvatar pAvatar) {
		base.ToggleContext(pShow, pAvatar);

		ContextWindow.Instance.GetContext<HackerContext>().gameObject.SetActive(pShow);
	}
}
