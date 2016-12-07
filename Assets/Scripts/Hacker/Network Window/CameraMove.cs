using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraMove : MonoBehaviour {
	[SerializeField]
	private float _moveSpeed;

	[SerializeField]
	private float _zoomSpeed;

	private Camera _camera;

	private void Start() {
		_camera = GetComponent<Camera>();
	}

	public void MoveTo (float x, float y) {
		transform.DOMove(new Vector3(x, y, transform.position.z), 0.1f);
	}

	private void Update() {
		if (_camera.pixelRect.Contains(Input.mousePosition)) {
			_camera.orthographicSize = Mathf.Max(1, _camera.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed * Time.deltaTime);

			if (Input.GetMouseButton(1)) {
				transform.Translate(new Vector3(-Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"), 0.0f) * _moveSpeed * Time.deltaTime * (_camera.orthographicSize * 0.01f));
			}
		}
	}
}
