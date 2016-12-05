using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class NodeContext : MonoBehaviour {
	public static NodeContext Instance;

	private ContextProgressBar _bar;

	private void Start () {
		_bar = GetComponentInChildren<ContextProgressBar>();
	}

	public void SetHackProgress (float pProgress) {
		_bar.SetProgress(pProgress);
	}

	private void Awake () {
		Instance = this;
		gameObject.SetActive(false);
	}
}
