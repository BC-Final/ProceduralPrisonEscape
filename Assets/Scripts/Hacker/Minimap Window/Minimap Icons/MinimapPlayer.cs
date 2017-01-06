using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MinimapPlayer : MonoBehaviour {
	private Vector3 _oldPos;
	private Vector3 _newPos;

	private float _lastUpdateTime = 0.0f;

	private float _currentLerpTime = 0.0f;
	private float _lerpTime = 0.5f;

	// Update is called once per frame
	private void Update () {
		_currentLerpTime += Time.deltaTime;
		if (_currentLerpTime > _lerpTime) {
			_currentLerpTime = _lerpTime;
		}

		float perc = _currentLerpTime / _lerpTime;
		transform.position = Vector3.Lerp(_oldPos, _newPos, perc);
	}

	public void InitialPosition (Vector3 pPos) {
		_lastUpdateTime = Time.time;
		_oldPos = pPos;
		_newPos = pPos;
	}

	public void UpdatePosition (Vector3 pPos) {
		_lerpTime = Time.time - _lastUpdateTime;
		_lastUpdateTime = Time.time;
		_currentLerpTime = 0f;
		_oldPos = transform.position;
		_newPos = pPos;
	}

	public void UpdateRotation (float pRot) {
		transform.rotation = Quaternion.Euler(0, pRot, 0);
	}
}