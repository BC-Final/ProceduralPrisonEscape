using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackerResourceUI : MonoBehaviour {
	private Text _text;

	private void Start() {
		_text = GetComponent<Text>();
	}

	private void Update() {
		_text.text = HackerResourceManager.Instance.HackerPoints.ToString();
	}
}
