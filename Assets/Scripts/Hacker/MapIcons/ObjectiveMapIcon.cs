using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveMapIcon : AbstractMapIcon {
	public static void ProcessPacket(NetworkPacket.GameObjects.Objective.Creation pPacket) {
		ObjectiveMapIcon icon = HackerPackageSender.GetNetworkedObject<ObjectiveMapIcon>(pPacket.Id);

		if (icon == null) {
			createInstance(pPacket);
		} else {
			Debug.LogError("Objective with Id " + pPacket.Id + " already exists.");
		}
	}

	public static void ProcessPacket (NetworkPacket.GameObjects.Objective.sUpdate pPacket) {
		ObjectiveMapIcon icon = HackerPackageSender.GetNetworkedObject<ObjectiveMapIcon>(pPacket.Id);

		if (icon != null) {
			icon.updateInstance(pPacket);
		} else {
			Debug.LogError("Objective with Id " + pPacket.Id + " does not exist");
		}
	}

	private static void createInstance (NetworkPacket.GameObjects.Objective.Creation pPacket) {
		ObjectiveMapIcon icon = Instantiate(HackerReferenceManager.Instance.ObjectiveIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.identity).GetComponent<ObjectiveMapIcon>();

		icon.Id = pPacket.Id;
	}

	private void updateInstance (NetworkPacket.GameObjects.Objective.sUpdate pPacket) {
		_lerpTime = Time.time - _lastUpdateTime;
		_lastUpdateTime = Time.time;
		_currentLerpTime = 0f;

		_oldPos = transform.position;
		_newPos = new Vector3(pPacket.PosX, pPacket.PosY, 0.0f) / MinimapManager.scale;

		if (pPacket.Finished) {
			Destroy(this.gameObject);
		}

		//determineSprite(pPacket.Fi);
	}

	private void Start () {
		_lastUpdateTime = Time.time;

		_oldPos = transform.position;
		_newPos = transform.position;
	}

	private Vector3 _oldPos;
	private Vector3 _newPos;

	private float _lastUpdateTime = 0.0f;

	private float _currentLerpTime = 0.0f;
	private float _lerpTime = 0.5f;

	private void Update () {
		_currentLerpTime += Time.deltaTime;
		if (_currentLerpTime > _lerpTime) {
			_currentLerpTime = _lerpTime;
		}

		if (_lerpTime == 0.0f) {
			_lerpTime = 0.5f;
		}

		float perc = _currentLerpTime / _lerpTime;

		transform.position = Vector3.Lerp(_oldPos, _newPos, perc);
	}
}
