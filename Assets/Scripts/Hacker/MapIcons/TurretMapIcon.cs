using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class TurretMapIcon : AbstractMapIcon {
	[SerializeField]
	private Sprite _neutralSprite;

	[SerializeField]
	private Sprite _disabledSprite;

	[SerializeField]
	private Sprite _controlledSprite;

	[SerializeField]
	private Sprite _seesPlayerSprite;

	public static void ProcessPacket(NetworkPacket.GameObjects.Turret.Creation pPacket) {
		TurretMapIcon icon = HackerPackageSender.GetNetworkedObject<TurretMapIcon>(pPacket.Id);

		if (icon == null) {
			createInstance(pPacket);
		} else {
			Debug.LogError("Module with Id " + pPacket.Id + " already exists.");
		}
	}

	public static void ProcessPacket (NetworkPacket.GameObjects.Turret.sUpdate pPacket) {
		TurretMapIcon icon = HackerPackageSender.GetNetworkedObject<TurretMapIcon>(pPacket.Id);

		if (icon != null) {
			icon.updateInstance(pPacket);
		} else {
			Debug.LogError("Module with Id " + pPacket.Id + " does not exist");
		}
	}

	private static void createInstance (NetworkPacket.GameObjects.Turret.Creation pPacket) {
		TurretMapIcon icon = Instantiate(HackerReferenceManager.Instance.TurretIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, -pPacket.Rot)).GetComponent<TurretMapIcon>();

		icon.Id = pPacket.Id;

		icon.determineSprite(pPacket.State);
	}

	private void updateInstance (NetworkPacket.GameObjects.Turret.sUpdate pPacket) {
		_lerpTime = Time.time - _lastUpdateTime;
		_lastUpdateTime = Time.time;
		_currentLerpTime = 0f;

		_oldRot = transform.rotation;
		_newRot = Quaternion.Euler(0, 0, -pPacket.Rot);

		determineSprite(pPacket.State);
	}

	private void Start () {
		_lastUpdateTime = Time.time;

		_oldRot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
		_newRot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
	}

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

		transform.rotation = Quaternion.Slerp(_oldRot, _newRot, perc);
	}

	private void determineSprite (EnemyState pState) {
		switch (pState) {
			case EnemyState.None:
				changeSprite(_neutralSprite);
				break;
			case EnemyState.SeesPlayer:
				changeSprite(_seesPlayerSprite);
				break;
			case EnemyState.Stunned:
				changeSprite(_disabledSprite);
				break;
			case EnemyState.Controlled:
				changeSprite(_controlledSprite);
				break;
		}
	}

	public void Disable () {
		sendUpdate(EnemyState.Stunned);
		determineSprite(EnemyState.Stunned);
	}

	public void Control () {
		sendUpdate(EnemyState.Controlled);
		determineSprite(EnemyState.Controlled);
	}

	private void sendUpdate (EnemyState pState) {
		HackerPackageSender.SendPackage(new NetworkPacket.Update.Turret(Id, pState));
	}
}
