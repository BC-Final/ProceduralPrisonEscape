using UnityEngine;
using System.Collections;

public class MinimapManager : HackerMapManager {

	[SerializeField]
	private GameObject _playerPrefab;
	[SerializeField]
	private GameObject _minimapDoorPrefab;
	[SerializeField]
	private GameObject _minimapFirewallPrefab;

	MinimapPlayer _player;


	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			CameraGoToPosition(_player.transform.position);
		}
			if (Input.GetMouseButtonDown(0))
			{
			RaycastHit hit;
			Ray ray = _camera.GetCamera().ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit))
				{
					//Debug.Log("My ray is created");
				if (hit.transform.GetComponent<MinimapDoor>())
				{
					//Debug.Log("My object is clicked by mouse");
					MinimapDoor door = hit.transform.GetComponent<MinimapDoor>();
					door.OnMouseClick();
				}
				//Debug.Log(hit.transform.name);
				}
			}
		}

	public MinimapDoor CreateMinimapDoor(Vector3 pos, float rotation, int ID)
	{
		Debug.Log("Doorrotaion" + rotation);
		GameObject gameObject = (GameObject)Instantiate(_minimapDoorPrefab, pos, Quaternion.Euler(0, rotation, 0));
		MinimapDoor miniDoor = gameObject.GetComponent<MinimapDoor>();
		miniDoor.SetManger(this);
		return miniDoor;
	}
	
	public MinimapFirewall CreateMinimapFirewall(Vector3 pos, int ID)
	{
		GameObject gameObject = (GameObject)Instantiate(_minimapFirewallPrefab, pos, Quaternion.Euler(0, 0, 0));
		MinimapFirewall miniWall = gameObject.GetComponent<MinimapFirewall>();
		return miniWall;
	}

	public void SendDoorUpdate(Door door)
	{
		_sender.SendDoorUpdate(door);
	}

	public void UpdateMinimapPlayer(Vector3 pos, float rotation)
	{
		if (_player == null)
		{
			GameObject gameObject = (GameObject)Instantiate(_playerPrefab, pos/scale, Quaternion.Euler(rotation, 0, 0));
			_player = gameObject.GetComponent<MinimapPlayer>();
		}
		else
		{
			_player.SetNewPos(pos/scale);
			_player.SetNewRotation(rotation);
		}
	}
}
