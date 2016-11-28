using UnityEngine;
using System.Collections;

public class MinimapPlayer : MonoBehaviour {

	private Vector3 oldPos;
	private Vector3 newPos;

	private float _lerpTime = 0.5f;
	private float _currentLerpTime;
	private float _timeSinceLastUpdate;

	// Update is called once per frame
	void Update () {
		_timeSinceLastUpdate += Time.deltaTime;
		//increment timer once per frame
		_currentLerpTime += Time.deltaTime;
		if (_currentLerpTime > _lerpTime)
		{
			_currentLerpTime = _lerpTime;
		}

		//lerp!
		float perc = _currentLerpTime / _lerpTime;
		transform.position = Vector3.Lerp(oldPos, newPos, perc);
	}

	public void SetNewPos(Vector3 nPos)
	{
		_lerpTime = _timeSinceLastUpdate;
		_timeSinceLastUpdate = 0f;
		_currentLerpTime = 0f;
		oldPos = newPos;
		newPos = nPos;
	}
	public void SetNewRotation(float rotation)
	{
		transform.rotation = Quaternion.Euler(0, rotation, 0);
	}
}