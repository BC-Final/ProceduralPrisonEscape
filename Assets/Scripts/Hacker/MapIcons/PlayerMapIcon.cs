using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapIcon : AbstractMapIcon {
	public static void ProcessPacket (NetworkPacket.Update.Player pPacket) {
		PlayerMapIcon icon = HackerPackageSender.GetNetworkedObject<PlayerMapIcon>(pPacket.Id);

		if (icon == null) {
			Debug.Log("Create" + pPacket.Id);
			createInstance(pPacket);
		} else {
			Debug.Log("Update");
			icon.updateInstance(pPacket);
		}
	}

	private static void createInstance (NetworkPacket.Update.Player pPacket) {
		PlayerMapIcon icon = Instantiate(HackerReferenceManager.Instance.PlayerIcon, new Vector3(pPacket.PosX, pPacket.PosY, 0) / MinimapManager.scale, Quaternion.Euler(0, 0, -pPacket.Rot)).GetComponent<PlayerMapIcon>();

		icon._health = pPacket.Health;
		icon.Id = pPacket.Id;
	}

	private void updateInstance (NetworkPacket.Update.Player pPacket) {
		_lerpTime = Time.time - _lastUpdateTime;
		_lastUpdateTime = Time.time;
		_currentLerpTime = 0f;

		_oldPos = transform.position;
		_newPos = new Vector3(pPacket.PosX, pPacket.PosY, 0.0f) / MinimapManager.scale;

		_oldRot = transform.rotation;
		_newRot = Quaternion.Euler(0, 0, -pPacket.Rot);

		_health = pPacket.Health;
	}

	private void Start () {
		_lastUpdateTime = Time.time;

		_oldPos = transform.position;
		_newPos = transform.position;

		_oldRot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
		_newRot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
	}

	private float _health;

	private Vector3 _oldPos;
	private Vector3 _newPos;

	private Quaternion _oldRot;
	private Quaternion _newRot;

	private float _lastUpdateTime = 0.0f;

	private float _currentLerpTime = 0.0f;
	private float _lerpTime = 0.1f;


	private void Update () {
		_currentLerpTime += Time.deltaTime;
		if (_currentLerpTime > _lerpTime) {
			_currentLerpTime = _lerpTime;
		}

		if (_lerpTime == 0.0f) {
			_lerpTime = 0.1f;
		}

		float perc = _currentLerpTime / _lerpTime;

		transform.position = Vector3.Lerp(_oldPos, _newPos, perc);
		transform.rotation = Quaternion.Slerp(_oldRot, _newRot, perc);
	}
}
