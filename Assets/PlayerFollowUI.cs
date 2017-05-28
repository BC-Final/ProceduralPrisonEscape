using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFollowUI : MonoBehaviour {
	private CameraDragAndDrop _cam;
	private Text _text;

	private void Start() {
		_cam = FindObjectOfType<CameraDragAndDrop>();
		_text = GetComponentInChildren<Text>();
		_cam.FollowPlayer.OnValueChange += setText;

		setText();
	}

	private void setText() {
		if (_cam.FollowPlayer.Value) {
			_text.text = "ON";
			_text.color = Color.green;
		} else {
			_text.text = "OFF";
			_text.color = Color.red;
		}
	}

	public void OnClick() {
		_cam.SetFollowPlayer(!_cam.FollowPlayer.Value);
	}
}
