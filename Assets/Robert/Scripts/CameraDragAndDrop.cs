using UnityEngine;
using System.Collections;

public class CameraDragAndDrop : MonoBehaviour {

	[SerializeField]
	int mouseRegion;

	private bool _dragging = false;
	private Camera _camera;

	private Vector3 _targetPos;
	private Vector3 _startPos;
	private Vector3 _newPos;
	private Vector3 _startTransformPos;

	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera>();
		_targetPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && MouseIsInScreenRegion(mouseRegion))
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
			_targetPos = new Vector3(_startTransformPos.x + _newPos.x*scrollPos/50, 1, _startTransformPos.z + _newPos.y*scrollPos/50);		
		}
		this.transform.position = Vector3.Lerp(this.transform.position, _targetPos, Time.deltaTime * 0.985f);
	}
	public void SetTargetPos(Vector3 targetPosition)
	{
		_targetPos = targetPosition;
		_targetPos.y = 1;
	}

	public Camera GetCamera()
	{
		return _camera;
	}

	/// <summary>
	/// Returns true if mouse is in given part of the screen. 
	/// Each part is a quarter of the screen in width and height starting from bottom left, then bottom right, then top left and top right.
	/// 3|4
	/// -|-
	/// 1|2
	/// 
	/// </summary>
	/// <param name="regionNumber"></param>
	/// <returns></returns>
	bool MouseIsInScreenRegion(int regionNumber)
	{
		if(Input.mousePosition.x < Screen.width/2 && Input.mousePosition.x > 0 && Input.mousePosition.y < Screen.height/2 && Input.mousePosition.y > 0)
		{
			if(regionNumber == 1)
			{
				return true;
			}
		}
		if (Input.mousePosition.x < Screen.width && Input.mousePosition.x > Screen.width/2 && Input.mousePosition.y < Screen.height / 2 && Input.mousePosition.y > 0)
		{
			if (regionNumber == 2)
			{
				return true;
			}
		}
		if (Input.mousePosition.x < Screen.width / 2 && Input.mousePosition.x > 0 && Input.mousePosition.y < Screen.height && Input.mousePosition.y > Screen.height/2)
		{
			if (regionNumber == 3)
			{
				return true;
			}
		}
		if (Input.mousePosition.x < Screen.width && Input.mousePosition.x > Screen.height/2 && Input.mousePosition.y < Screen.height && Input.mousePosition.y > Screen.height/2)
		{
			if (regionNumber == 4)
			{
				return true;
			}
		}
		return false;
	}

	void StartDragging()
	{
		_dragging = true;
	}

	void StopDragging()
	{
		_targetPos = this.transform.position;
		_dragging = false;
	}
}
