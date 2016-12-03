using UnityEngine;
using System.Collections;

public class MinimapManager : HackerMapManager {

	public static MinimapManager _instance;

	[SerializeField]
	private GameObject _playerPrefab;
	[SerializeField]
	private GameObject _minimapDoorPrefab;
	[SerializeField]
	private GameObject _minimapFirewallPrefab;
	[SerializeField]
	private GameObject _minimapPickupIconPrefab;

	MinimapPlayer _player;

	[SerializeField]
	private float _iconHeight = 1;

	protected override void Start () {
		base.Start();
	}

	// Update is called once per frame
	void Update () {
		if (_camera.GetCamera().pixelRect.Contains(Input.mousePosition)) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				CameraGoToPosition(_player.transform.position);
			}

			if (Input.GetMouseButtonDown(0)) {
				RaycastHit hit;
				Ray ray = _camera.GetCamera().ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit)) {
					//Debug.Log("My ray is created");
					if (hit.transform.GetComponent<MinimapDoor>()) {
						//Debug.Log("My object is clicked by mouse");
						MinimapDoor door = hit.transform.GetComponent<MinimapDoor>();
						door.OnMouseClick();
					}
					//Debug.Log(hit.transform.name);
				}
			}
		}
	}

	public MinimapDoor CreateMinimapDoor (Vector3 pos, float rotation, int ID) {
		pos.y = _iconHeight;
		GameObject gameObject = (GameObject)Instantiate(_minimapDoorPrefab, pos / scale, Quaternion.Euler(0, rotation, 0));
		MinimapDoor miniDoor = gameObject.GetComponent<MinimapDoor>();
		return miniDoor;
	}

	public MinimapFirewall CreateMinimapFirewall (Vector3 pos, int ID) {
		pos.y = _iconHeight;
		GameObject gameObject = (GameObject)Instantiate(_minimapFirewallPrefab, pos / scale, Quaternion.Euler(0, 0, 0));
		MinimapFirewall miniWall = gameObject.GetComponent<MinimapFirewall>();
		return miniWall;
	}

	public MinimapPickupIcon CreateMinimapIcon (Vector3 pos) {
		GameObject gameObject = (GameObject)Instantiate(_minimapPickupIconPrefab, pos / scale, Quaternion.Euler(0, 0, 0));
		return gameObject.GetComponent<MinimapPickupIcon>();
	}
	public MinimapEnemy CreateMinimapEnemy (Vector3 pos) {
		GameObject gameObject = (GameObject)Instantiate(HackerReferenceManager.Instance.EnemyIcon, pos / scale, Quaternion.Euler(0, 0, 0));
		return gameObject.GetComponent<MinimapEnemy>();
	}



	public void UpdateMinimapPlayer (CustomCommands.Update.PlayerPositionUpdate package) {
		Vector3 pos = new Vector3(package.x, _iconHeight, package.z);

		if (_player == null) {
			GameObject gameObject = (GameObject)Instantiate(_playerPrefab, pos / scale, Quaternion.Euler(0, package.rotation, 0));
			_player = gameObject.GetComponent<MinimapPlayer>();
			_player.InitialPosition(pos / scale);
		} else {
			_player.SetNewPos(pos / scale);
			_player.SetNewRotation(package.rotation);
		}
	}


	public void CreateShot (CustomCommands.Creation.Shots.LaserShotCreation package) {
		Vector3 startPos = new Vector3(package.startX, 1, package.startZ);
		Vector3 endPos = new Vector3(package.targetX, 1, package.targetZ);
		GameObject gameObject = (GameObject)Instantiate(HackerReferenceManager.Instance.LaserShot, startPos / scale, Quaternion.Euler(0, 0, 0));
		LaserShot shot = gameObject.GetComponent<LaserShot>();
		shot.SetTargetPos(endPos / scale);
	}

	//Static
	public static MinimapManager GetInstance () {
		if (_instance == null) {
			_instance = FindObjectOfType<MinimapManager>();
			if (_instance == null) {
				Debug.Log("ERROR!!! MINIMAP MANAGER NOT FOUND");
			}
		}
		return _instance;
	}
}
