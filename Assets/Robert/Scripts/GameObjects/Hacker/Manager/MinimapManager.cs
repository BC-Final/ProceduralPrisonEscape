using UnityEngine;
using System.Collections;

public class MinimapManager : HackerMapManager {

	[SerializeField]
	private GameObject _playerPrefab;
	[SerializeField]
	private GameObject _minimapDoorPrefab;

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
	}

	public MinimapDoor CreateMinimapDoor(Vector3 pos, float rotation, int ID)
	{
		GameObject gameObject = (GameObject)Instantiate(_minimapDoorPrefab, pos, Quaternion.Euler(0, rotation, 0));
		return gameObject.GetComponent<MinimapDoor>();
	}

	public void UpdateMinimapPlayer(Vector3 pos)
	{
		if (_player == null)
		{
			GameObject gameObject = (GameObject)Instantiate(_playerPrefab, pos/_scale, Quaternion.identity);
			_player = gameObject.GetComponent<MinimapPlayer>();
		}
		else
		{
			_player.SetNewPos(pos/_scale);
		}
	}
}
