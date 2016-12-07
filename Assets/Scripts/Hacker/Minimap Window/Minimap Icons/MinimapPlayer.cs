using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MinimapPlayer : MonoBehaviour {
	private Vector3 oldPos;
	private Vector3 newPos;
	private float _lerpTime = 0.1f;
	private float _currentLerpTime;
	private float _timeSinceLastUpdate = 0.1f;

	// Update is called once per frame
	void Update () {
		_timeSinceLastUpdate += Time.deltaTime;
		//increment timer once per frame
		_currentLerpTime += Time.deltaTime;
		if (_currentLerpTime > _lerpTime) {
			_currentLerpTime = _lerpTime;
		}

		//lerp!
		float perc = _currentLerpTime / _lerpTime;
		transform.position = Vector3.Lerp(oldPos, newPos, perc);
	}

	public void InitialPosition (Vector3 pPosition) {
		oldPos = pPosition;
		newPos = pPosition;
	}
	

	public void SetNewPos (Vector3 nPos) {
		_lerpTime = _timeSinceLastUpdate;
		_timeSinceLastUpdate = 0f;
		_currentLerpTime = 0f;
		oldPos = newPos;
		newPos = nPos;
	}

	public void SetNewRotation (float rotation) {
		transform.rotation = Quaternion.Euler(0, rotation, 0);
	}
	
}