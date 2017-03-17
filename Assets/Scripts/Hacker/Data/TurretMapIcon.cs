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

	public static void ProcessPacket (NetworkPacket.Update.Turret pPacket) {
		TurretMapIcon icon = HackerPackageSender.GetNetworkedObject<TurretMapIcon>(pPacket.Id);

		if (icon == null) {
			createInstance(pPacket);
		} else {
			icon.updateInstance(pPacket);
		}
	}

	private static void createInstance (NetworkPacket.Update.Turret pPacket) {
		TurretMapIcon icon = Instantiate(HackerReferenceManager.Instance.TurretIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, -pPacket.Rot)).GetComponent<TurretMapIcon>();

		icon._state.OnValueChange += icon.changedState;

		icon.Id = pPacket.Id;
		icon._state.Value = pPacket.State;
	}

	private void updateInstance (NetworkPacket.Update.Turret pPacket) {
		_lerpTime = Time.time - _lastUpdateTime;
		_lastUpdateTime = Time.time;
		_currentLerpTime = 0f;

		_oldRot = transform.rotation;
		_newRot = Quaternion.Euler(0, 0, -pPacket.Rot);
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

	private ObservedValue<EnemyState> _state = new ObservedValue<EnemyState>(EnemyState.None);

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

	private void changedState () {
		//TODO Display a state change
	}

	public void Disable () {
		sendUpdate(EnemyState.Stunned);
	}

	public void Control () {
		sendUpdate(EnemyState.Controlled);
	}

	private void sendUpdate (EnemyState pState) {
		HackerPackageSender.SendPackage(new NetworkPacket.Update.Turret(Id, pState));
	}
}
