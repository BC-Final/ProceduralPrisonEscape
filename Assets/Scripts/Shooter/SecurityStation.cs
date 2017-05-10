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

	private bool _pressable;
	private bool _pressed;

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
		
	}

	//TODO Make Networked
	private void Start() {
		_light = GetComponentInChildren<Light>(true);
		ShooterAlarmManager.Instance.OnAlarmChange += onAlarmChange;
	}

	public static void ProcessPacket(NetworkPacket.Update.SecurityStation pPacket) {
		SecurityStation station = ShooterPackageSender.GetNetworkedObject<SecurityStation>(pPacket.Id);

		if (station != null) {

		}
	}

	public void Interact() {
		if(_pressed)
	}

	private void setNotPressable() {
		_pressable = false;
		TimerManager.CreateTimer("Security Station Cooldown", true).SetDuration(_)
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
