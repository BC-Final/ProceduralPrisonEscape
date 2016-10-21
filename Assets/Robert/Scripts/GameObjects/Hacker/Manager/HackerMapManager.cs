using UnityEngine;
using System.Collections;

public class HackerMapManager : MonoBehaviour {
	
	protected float _scale;
	[SerializeField]
	CameraDragAndDrop _camera;
	// Use this for initialization
	protected virtual void Start () {
		_camera = GetComponentInChildren<CameraDragAndDrop>();
	}
	
	public void SetScale(float scale)
	{
		_scale = scale;
	}

	public void CameraGoToPosition(Vector3 pos)
	{
		_camera.SetTargetPos(pos);
	}

}
