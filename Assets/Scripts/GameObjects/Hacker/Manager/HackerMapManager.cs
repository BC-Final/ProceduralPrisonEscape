using UnityEngine;
using System.Collections;

public class HackerMapManager : MonoBehaviour {
	
	public float scale;
	[SerializeField]
	protected CameraDragAndDrop _camera;
	[SerializeField]
	protected HackerPackageSender _sender;
	// Use this for initialization
	protected virtual void Start () {
		_camera = GetComponentInChildren<CameraDragAndDrop>();
	}
	
	public void SetScale(float scale)
	{
		this.scale = scale;
	}

	public void SetSender(HackerPackageSender sender)
	{
		_sender = sender;
	}

	public void CameraGoToPosition(Vector3 pos)
	{
		_camera.SetTargetPos(pos);
	}

}
