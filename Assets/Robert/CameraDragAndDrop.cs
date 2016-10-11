using UnityEngine;
using System.Collections;

public class CameraDragAndDrop : MonoBehaviour {

	bool _dragging = false;
	Camera _camera;

	Vector3 _startPos;
	Vector3 _newPos;
	Vector3 _startTransformPos;

	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			StartDragging();
			_startPos = Input.mousePosition;
			_startTransformPos = this.transform.position;
		}
		if (Input.GetMouseButtonUp(0))
		{
			StopDragging();
		}

		float scrollPos = _camera.orthographicSize - Input.mouseScrollDelta.y * 0.5f;
		_camera.orthographicSize = Mathf.Clamp(scrollPos, 1f, 15f); ;

		if (_dragging)
		{
			_newPos = _startPos - Input.mousePosition;
			this.transform.position = new Vector3(_startTransformPos.x + _newPos.x*scrollPos/50, 1, _startTransformPos.z + _newPos.y*scrollPos/50);
		}
	}

	void StartDragging()
	{
		_dragging = true;
	}

	void StopDragging()
	{
		_dragging = false;
	}
}
