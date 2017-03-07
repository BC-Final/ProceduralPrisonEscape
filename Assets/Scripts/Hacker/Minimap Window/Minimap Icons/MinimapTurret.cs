using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapTurret : MonoBehaviour {
	private HackerTurret _associatedTurret;

	public HackerTurret AssociatedTurret {
		set { _associatedTurret = value; }
	}

	private Quaternion _oldRot;
	private Quaternion _newRot;

	private float _lastUpdateTime = 0.0f;

	private float _currentLerpTime = 0.0f;
	private float _lerpTime = 0.5f;

	private void Update () {
		_currentLerpTime += Time.deltaTime;
		if (_currentLerpTime > _lerpTime) {
			_currentLerpTime = _lerpTime;
		}

		if (_lerpTime == 0.0f) {
			_lerpTime = 0.5f;
		}

		float perc = _currentLerpTime / _lerpTime;

		transform.rotation = Quaternion.Slerp(_oldRot, _newRot, perc);
	}

	public void InitialTransform (float pRot) {
		_lastUpdateTime = Time.time;

		_oldRot = Quaternion.Euler(0, pRot, 0);
		_newRot = Quaternion.Euler(0, pRot, 0);
	}

	public void UpdateTransform (float pRot) {
		_lerpTime = Time.time - _lastUpdateTime;
		_lastUpdateTime = Time.time;
		_currentLerpTime = 0f;

		_oldRot = transform.rotation;
		_newRot = Quaternion.Euler(0, pRot, 0);
	}
}
