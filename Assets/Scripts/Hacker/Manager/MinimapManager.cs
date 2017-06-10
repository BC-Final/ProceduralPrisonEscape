using UnityEngine;
using System.Collections;
using Gamelogic.Extensions;

public class MinimapManager : Singleton<MinimapManager> {

    //MinimapPlayer _player;

    [SerializeField]
    static public float scale = 2f;
	[SerializeField]
	protected CameraDragAndDrop _camera;
	// Use this for initialization
	protected virtual void Start () {
		_camera = GetComponentInChildren<CameraDragAndDrop>();
	}

	public void SetScale (float scale) {
		//this.scale = scale;
	}


	public void CameraGoToPosition (Vector3 pos) {
		_camera.SetTargetPos(pos);
	}

	[SerializeField]
	private float _iconHeight = 1;

	// Update is called once per frame
	//void Update () {
	//	if (_camera.GetCamera().pixelRect.Contains(Input.mousePosition)) {
	//		if (Input.GetKeyDown(KeyCode.Space)) {
	//			//TODO Replace
	//			//CameraGoToPosition(_player.transform.position);
	//		}

	//		if (Input.GetMouseButtonDown(0)) {
	//			RaycastHit hit;
	//			Ray ray = _camera.GetCamera().ScreenPointToRay(Input.mousePosition);

	//			if (Physics.Raycast(ray, out hit)) {
	//				//Debug.Log("My ray is created");
	//				if (hit.transform.GetComponent<MinimapDoor>()) {
	//					//Debug.Log("My object is clicked by mouse");
	//					MinimapDoor door = hit.transform.GetComponent<MinimapDoor>();
	//					door.OnMouseClick();
	//				}
	//				//Debug.Log(hit.transform.name);
	//			}
	//		}
	//	}
	//}

	//TODO Revamp these functions
	//public MinimapDoor CreateMinimapDoor (Vector3 pos, float rotation, int ID) {
	//	//pos.y = _iconHeight;
	//	GameObject gameObject = (GameObject)Instantiate(HackerReferenceManager.instance.DoorIcon, pos / scale, Quaternion.Euler(0, rotation, 0));
	//	return gameObject.GetComponent<MinimapDoor>();
	//}

	//public MinimapFirewall CreateMinimapFirewall (Vector3 pos, int ID) {
	//	//pos.y = _iconHeight;
	//	GameObject go = (GameObject)Instantiate(HackerReferenceManager.instance.FireWallIcon, pos / scale, Quaternion.identity);
	//	return go.GetComponent<MinimapFirewall>();
	//}


	//public MinimapPickupIcon CreateMinimapIcon (Vector3 pos) {
	//	GameObject go = (GameObject)Instantiate(HackerReferenceManager.instance.PickupIcon, pos / scale, Quaternion.identity);
	//	return go.GetComponent<MinimapPickupIcon>();
	//}

	//public MinimapDrone CreateMinimapDrone (float pX, float pZ, float pRot) {
	//	GameObject go = (GameObject)Instantiate(HackerReferenceManager.Instance.DroneIcon, new Vector3(pX, 0, pZ) / scale, Quaternion.Euler(0, pRot, 0));
	//	return go.GetComponent<MinimapDrone>();
	//}

	//public MinimapCamera CreateMinimapCamera (float pX, float pZ, float pRot) {
	//	GameObject go = (GameObject)Instantiate(HackerReferenceManager.Instance.CameraIcon, new Vector3(pX, 0 ,pZ) / scale, Quaternion.Euler(0, pRot, 0));
	//	return go.GetComponent<MinimapCamera>();
	//}

	//public MinimapTurret CreateMinimapTurret (float pX, float pZ, float pRot) {
	//	GameObject go = (GameObject)Instantiate(HackerReferenceManager.Instance.TurretIcon, new Vector3(pX, 0, pZ) / scale, Quaternion.Euler(0, pRot, 0));
	//	return go.GetComponent<MinimapTurret>();
	//}


	//TODO THis belongs somehweewreelse boiii
	//public void UpdateMinimapPlayer (CustomCommands.Update.PlayerPositionUpdate package) {
	//	Vector3 pos = new Vector3(package.x, _iconHeight, package.z);

	//	if (_player == null) {
	//		GameObject gameObject = (GameObject)Instantiate(HackerReferenceManager.instance.PlayerIcon, pos / scale, Quaternion.Euler(0, package.rotation, 0));
	//		_player = gameObject.GetComponent<MinimapPlayer>();
	//		_player.InitialTransform(pos / scale, package.rotation);
	//	} else {
	//		_player.UpdateTransform(pos / scale, package.rotation);
	//	}
	//}


	public void CreateShot (NetworkPacket.GameObjects.Lasershot.Creation pPacket) {
		Vector3 startPos = new Vector3(pPacket.startX, pPacket.startZ, 1);
		Vector3 endPos = new Vector3(pPacket.targetX, pPacket.targetZ, 1);
		GameObject gameObject = (GameObject)Instantiate(HackerReferenceManager.Instance.LaserShot, startPos / scale, Quaternion.Euler(0, 0, 0));
		LaserShot shot = gameObject.GetComponent<LaserShot>();
		shot.SetTargetPos(endPos / scale);
	}

	public void ProcessPacket(NetworkPacket.Messages.MoveCameraTowardsLocation pPacket)
	{
		Vector3 targetPos = (new Vector3(pPacket.posX, pPacket.posY, 0) / scale);
		Debug.Log("packet pos	: " + "x" + pPacket.posX + "/Y" + pPacket.posY);
		Debug.Log("target pos	: " + targetPos);
		Debug.Log("scale		: " + scale);
		_camera.SetTargetPos(targetPos);
		targetPos.z = -7f;
		MapSelectionHighlight circle = Instantiate(HackerReferenceManager.Instance.HighlightingCircle, targetPos, Quaternion.Euler(0, 0, 0)).GetComponent<MapSelectionHighlight>();
		circle.IndicateSelection(targetPos);
	}
}
