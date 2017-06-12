using UnityEngine;
using System.Collections;
using Gamelogic.Extensions;

public class CameraDragAndDrop : MonoBehaviour {
	private bool _dragging = false;
	private Camera _camera;
	private ObservedValue<bool> _followPlayer = new ObservedValue<bool>(true);
	public ObservedValue<bool> FollowPlayer { get { return _followPlayer; } }
	public void SetFollowPlayer(bool pValue) { _followPlayer.Value = pValue; }
	private Transform _player;

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
	void Start() {
		_camera = GetComponent<Camera>();
		_targetPos = this.transform.position;
	}

	// Update is called once per frame
	void Update() {

		//enable Player following
		if (Input.GetKeyDown(KeyCode.Z)) {
			if (_player == null) {
				_player = GameObject.FindObjectOfType<PlayerMapIcon>().transform;
			}

			_followPlayer.Value = !_followPlayer.Value;
			_currentLerpTime = 0f;
		}

		float scrollPos = _camera.orthographicSize;

		if (_camera.pixelRect.Contains(Input.mousePosition)) {
			if (Input.GetMouseButtonDown(1)) {
				_dragging = true;
				_startPos = Input.mousePosition;
				_startTransformPos = this.transform.position;
			}

			//Scrolling mouse wheel
			scrollPos = _camera.orthographicSize - Input.mouseScrollDelta.y * 0.5f;
			_camera.orthographicSize = Mathf.Clamp(scrollPos, 1f, 15f);
		}

		if (_dragging) {
			//stop following player
			_followPlayer.Value = false;

			_currentLerpTime = 0f;
			_newPos = _startPos - Input.mousePosition;
			_targetPos = new Vector3(_startTransformPos.x + _newPos.x * scrollPos * (_mouseSensitivity / 10000), _startTransformPos.y + _newPos.y * scrollPos * (_mouseSensitivity / 10000), 0);
		}

		//if camera is following play target position is always the player
		//if (_followPlayer.Value) {
		//	if (_player == null) {
		//		PlayerMapIcon icon = GameObject.FindObjectOfType<PlayerMapIcon>();

		//		if (icon != null) {
		//			_player = GameObject.FindObjectOfType<PlayerMapIcon>().transform;
		//		}
		//	}

		//	if (_player != null) {
		//		_targetPos = _player.transform.position;
		//	}
		//}

		if (!_followPlayer.Value) {
			_currentLerpTime += Time.deltaTime;
			if (_currentLerpTime > _lerpTime) {
				_currentLerpTime = _lerpTime;
			}
			float perc = _currentLerpTime / _lerpTime;

			//Set target position to 1 else camer might be buggy
			_targetPos.z = -15;
			this.transform.position = Vector3.Lerp(this.transform.position, _targetPos, perc);
		}

		if (_dragging && Input.GetMouseButtonUp(1)) {
			_targetPos = this.transform.position;
			_dragging = false;
		}
	}

	private void LateUpdate() {
		if (_followPlayer.Value) {
			if (_player == null) {
				PlayerMapIcon icon = GameObject.FindObjectOfType<PlayerMapIcon>();

				if (icon != null) {
					_player = GameObject.FindObjectOfType<PlayerMapIcon>().transform;
				}
			}

			if (_player != null) {
				_targetPos = _player.transform.position;
			}

			_currentLerpTime += Time.deltaTime;
			if (_currentLerpTime > _lerpTime) {
				_currentLerpTime = _lerpTime;
			}
			float perc = _currentLerpTime / _lerpTime;

			//Set target position to 1 else camer might be buggy
			_targetPos.z = -15;
			this.transform.position = Vector3.Lerp(this.transform.position, _targetPos, perc);
		}
	}

	public void SetTargetPos(Vector3 targetPosition) {
		_followPlayer.Value = false;
		_targetPos = targetPosition;
		_currentLerpTime = 0f;
	}
}
