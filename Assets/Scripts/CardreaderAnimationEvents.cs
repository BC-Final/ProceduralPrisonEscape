using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardreaderAnimationEvents : MonoBehaviour {
	[SerializeField]
	private GameObject _keycard;

	public void ToggleVisibility() {
		_keycard.SetActive(!_keycard.activeSelf);
	}
}
