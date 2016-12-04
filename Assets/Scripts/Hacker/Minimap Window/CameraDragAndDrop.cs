using UnityEngine;
using System.Collections;

public class CameraDragAndDrop : MonoBehaviour {
	private bool _dragging = false;
	private Camera _camera;

	private Vector3 _targetPos;
	private Vector3 _startPos;
	private Vector3 _newPos;
	private Vector3 _startTransformPos;

	[SerializeField]
	private float _mouseSensitivity = 10;
	[SerializeField]
	private float _lerpTime = 0.2f;
	private float _currentLerpTime = 0;

	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera>();
		_targetPos = this.transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (_camera.pixelRect.Contains(Input.mousePosition)) {
			if (Input.GetMouseButtonDown(1)) {
				StartDragging();
				_startPos = Input.mousePosition;
				_startTransformPos = this.transform.position;
			}

			//Scrolling mouse wheel
			float scrollPos = _camera.orthographicSize - Input.mouseScrollDelta.y * 0.5f;
			_camera.orthographicSize = Mathf.Clamp(scrollPos, 1f, 15f);

			if (_dragging) {
				_currentLerpTime = 0f;
				_newPos = _startPos - Input.mousePosition;
				_targetPos = new Vector3(_startTransformPos.x + _newPos.x * scrollPos * (_mouseSensitivity / 10000), 1, _startTransformPos.z + _newPos.y * scrollPos * (_mouseSensitivity / 10000));
			}
			_currentLerpTime += Time.deltaTime;
			if (_currentLerpTime > _lerpTime) {
				_currentLerpTime = _lerpTime;
			}
			float perc = _currentLerpTime / _lerpTime;
			this.transform.position = Vector3.Lerp(this.transform.position, _targetPos, perc);
		}

		if (Input.GetMouseButtonUp(1)) {
			StopDragging();
		}

	}

	public void SetTargetPos (Vector3 targetPosition) {
		_targetPos = targetPosition;
		_targetPos.y = 1;
	}

	public Camera GetCamera () {
		return _camera;
	}

	void StartDragging () {
		_dragging = true;
	}

	void StopDragging () {
		_targetPos = this.transform.position;
		_dragging = false;
	}
}
