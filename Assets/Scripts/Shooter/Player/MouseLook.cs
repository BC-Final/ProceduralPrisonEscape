using UnityEngine;

public class MouseLook : MonoBehaviour {
	private Vector2 _mouseAbsolute;
	private Vector2 _smoothMouse;

	[SerializeField]
	private Vector2 _clampInDegrees = new Vector2(360, 180);

	[SerializeField]
	private bool _lockCursor;

	[SerializeField]
	private Vector2 _sensitivity = new Vector2(2, 2);

	[SerializeField]
	private Vector2 _smoothing = new Vector2(3, 3);

	private Vector2 _targetDirection;
	private Vector2 _targetCharacterDirection;


	// Assign this if there's a parent object controlling motion, such as a Character Controller.
	// Yaw rotation will affect this object instead of the camera if set.
	public Transform characterBody;

	void Start() {
		// Set target direction to the camera's initial orientation.
		_targetDirection = transform.localRotation.eulerAngles;

		// Set target direction for the character body to its inital state.
		if (characterBody)
			_targetCharacterDirection = characterBody.localRotation.eulerAngles;
	}

	void Update() {
		if (_lockCursor) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		} else {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}


		// Allow the script to clamp based on a desired target value.
		var targetOrientation = Quaternion.Euler(_targetDirection);
		var targetCharacterOrientation = Quaternion.Euler(_targetCharacterDirection);

		// Get raw mouse input for a cleaner reading on more sensitive mice.
		var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

		// Scale input against the sensitivity setting and multiply that against the smoothing value.
		mouseDelta = Vector2.Scale(mouseDelta, new Vector2(_sensitivity.x * _smoothing.x, _sensitivity.y * _smoothing.y));

		// Interpolate mouse movement over time to apply smoothing delta.
		_smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / _smoothing.x);
		_smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / _smoothing.y);

		// Find the absolute mouse movement value from point zero.
		_mouseAbsolute += _smoothMouse;

		// Clamp and apply the local x value first, so as not to be affected by world transforms.
		if (_clampInDegrees.x < 360)
			_mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -_clampInDegrees.x * 0.5f, _clampInDegrees.x * 0.5f);

		var xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
		transform.localRotation = xRotation;

		// Then clamp and apply the global y value.
		if (_clampInDegrees.y < 360)
			_mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -_clampInDegrees.y * 0.5f, _clampInDegrees.y * 0.5f);

		transform.localRotation *= targetOrientation;

		// If there's a character body that acts as a parent to the camera
		if (characterBody) {
			var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, characterBody.up);
			characterBody.localRotation = yRotation;
			characterBody.localRotation *= targetCharacterOrientation;
		} else {
			var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
			transform.localRotation *= yRotation;
		}
	}
}