using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[SelectionBase]
public class SecurityStation : MonoBehaviour, IShooterNetworked, IInteractable {
	[SerializeField]
	private float _addedIntesity;

	[SerializeField]
	private float _interactThreshold;

	[SerializeField]
	private float _interactCooldown;

	private bool _sPressed = false, _hPressed = false;
	private bool _sCanPress = true, _hCanPress = true;

	private Light _light;



	private int _id;
	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}

	private void Awake() {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy() {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	public void Initialize() {
		ShooterPackageSender.SendPackage(new NetworkPacket.Create.SecurityStation(Id, transform.position.x, transform.position.z));
	}

	private void Start() {
		_light = GetComponentInChildren<Light>(true);
		ShooterAlarmManager.Instance.OnAlarmChange += onAlarmChange;
	}

	public static void ProcessPacket(NetworkPacket.Update.SecurityStation pPacket) {
		SecurityStation station = ShooterPackageSender.GetNetworkedObject<SecurityStation>(pPacket.Id);

		if (station != null) {
			station.hackerInteract();
		}
	}

	private void hackerInteract() {
		if (_hCanPress) {
			if (_sPressed) {
				ShooterAlarmManager.Instance.AlarmIsOn = false;
			} else {
				_hPressed = true;
				_hCanPress = false;
				TimerManager.CreateTimer("Security Station Threshold H", true).SetDuration(_interactThreshold).AddCallback(() => _hPressed = false).Start();
				TimerManager.CreateTimer("Security Station Cooldown H", true).SetDuration(_interactCooldown).AddCallback(() => _hCanPress = true).Start();
			}
		}
	}

	public void Interact() {
		if (_sCanPress) {
			if (_hPressed) {
				ShooterAlarmManager.Instance.AlarmIsOn = false;
			} else {
				_sPressed = true;
				_sCanPress = false;
				TimerManager.CreateTimer("Security Station Threshold S", true).SetDuration(_interactThreshold).AddCallback(() => _sPressed = false).Start();
				TimerManager.CreateTimer("Security Station Cooldown S", true).SetDuration(_interactCooldown).AddCallback(() => _sCanPress = true).Start();
			}
		}
	}

	private void onAlarmChange() {
		if (ShooterAlarmManager.Instance.AlarmIsOn) {
			_light.enabled = true;

			DOTween.Sequence()
			.Append(_light.DOIntensity(_light.intensity + _addedIntesity, 0.3f))
			.Append(_light.DOIntensity(_light.intensity, 0.3f))
			.SetLoops(-1)
			.SetTarget(_light);
		} else {
			_light.DOKill();
			_light.enabled = false;
		}
	}
}
