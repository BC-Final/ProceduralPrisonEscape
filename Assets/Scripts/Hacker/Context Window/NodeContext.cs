using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;
using UnityEngine.UI;
using System;

public class NodeContext : MonoBehaviour {
	[Serializable]
	public struct ContextButton {
		public string Name;
		public Button Button;
	}


	[SerializeField]
	private List<ContextButton> _contextButtons = new List<ContextButton>();

	private ContextProgressBar _bar;



	public void RegisterButton (string pName, UnityEngine.Events.UnityAction pDelegate) {
		_contextButtons.Find(x => x.Name == pName).Button.onClick.AddListener(pDelegate);
	}

	public void UnregisterAllButtons () {
		_contextButtons.ForEach(x => x.Button.onClick.RemoveAllListeners());
	}

	public void SetHackProgress (float pProgress) {
		_bar.SetProgress(pProgress);
	}



	private void Start () {
		_bar = GetComponentInChildren<ContextProgressBar>(true);
	}

	private void Awake () {
		gameObject.SetActive(false);
	}
}
