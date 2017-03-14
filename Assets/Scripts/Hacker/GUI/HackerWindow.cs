using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerWindow : MonoBehaviour {
	//TODO Add focus to windows (so they wont overlap when selected)
	protected HackerData _data;

	private void Start () {
		GetComponentInChildren<HackerWindowCloseButton>().GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => CloseWindow());
	}

	public void SetData (HackerData pData) {
		_data = pData;
	}

	public void CloseWindow () {
		Destroy(this.gameObject);
	}
}
