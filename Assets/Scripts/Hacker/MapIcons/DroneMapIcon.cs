using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class DroneMapIcon : AbstractMapIcon {
	[SerializeField]
	private Sprite _neutralSprite;

	[SerializeField]
	private Sprite _seesPlayerSprite;

	[SerializeField]
	private Sprite _deadSprite;

	[SerializeField]
	private Sprite _stunnedSprite;

	[SerializeField]
	private Sprite _controlledSprite;

	//TODO Add another image on top
	[SerializeField]
	private Sprite _shieldSprite;

	public static void ProcessPacket (NetworkPacket.GameObjects.Drone.sUpdate pPacket) {
		DroneMapIcon icon = HackerPackageSender.GetNetworkedObject<DroneMapIcon>(pPacket.Id);

		if (icon == null) {
			createInstance(pPacket);
		} else {
			icon.updateInstance(pPacket);
		}
	}

	private static void createInstance (NetworkPacket.GameObjects.Drone.sUpdate pPacket) {
		DroneMapIcon icon = Instantiate(HackerReferenceManager.Instance.DroneIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, -pPacket.Rot)).GetComponent<DroneMapIcon>();

		icon._health.Value = pPacket.Health;
		icon.Id = pPacket.Id;

		icon.determineSprite(pPacket.State);
	}

	private void updateInstance (NetworkPacket.GameObjects.Drone.sUpdate pPacket) {
		_lerpTime = Time.time - _lastUpdateTime;
		_lastUpdateTime = Time.time;
		_currentLerpTime = 0f;

		_oldPos = transform.position;
		_newPos = new Vector3(pPacket.PosX, pPacket.PosY, 0.0f) / MinimapManager.scale;

		_oldRot = transform.rotation;
		_newRot = Quaternion.Euler(0, 0, -pPacket.Rot);

		_health.Value = pPacket.Health;

		determineSprite(pPacket.State);
	}

	private void Start () {
		_lastUpdateTime = Time.time;

		_oldPos = transform.position;
		_newPos = transform.position;

		_oldRot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
		_newRot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
	}

	private ObservedValue<float> _health = new ObservedValue<float>(1.0f);

	private Vector3 _oldPos;
	private Vector3 _newPos;

	private Quaternion _oldRot;
	private Quaternion _newRot;

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
		transform.rotation = Quaternion.Slerp(_oldRot, _newRot, perc);
	}

	private void determineSprite (EnemyState pState) {
		if (_health.Value > 0.0f) {
			switch (pState) {
				case EnemyState.None:
					changeSprite(_neutralSprite);
					break;
				case EnemyState.SeesPlayer:
					changeSprite(_seesPlayerSprite);
					break;
				case EnemyState.Stunned:
					changeSprite(_stunnedSprite);
					break;
				case EnemyState.Controlled:
					changeSprite(_controlledSprite);
					break;
			}
		} else {
			changeSprite(_deadSprite);
            Destroy(gameObject, 6);
		}
	}
}

