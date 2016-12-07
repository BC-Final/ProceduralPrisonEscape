using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class NodeContext : MonoBehaviour {
	private ContextProgressBar _bar;

	private void Start () {
		_bar = GetComponentInChildren<ContextProgressBar>();
	}

	public void SetHackProgress (float pProgress) {
		_bar.SetProgress(pProgress);
	}

	protected virtual void Awake () {
		gameObject.SetActive(false);
	}
}
