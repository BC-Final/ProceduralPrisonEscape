using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Gamelogic.Extensions;

[SelectionBase]
public class ShooterSecurityStation : MonoBehaviour, IShooterNetworked, IInteractable {
	[SerializeField]
	private float _addedIntesity;

	[SerializeField]
	private float _interactThreshold;

	[SerializeField]
	private float _interactCooldown;

	private bool _sPressed = false, _hPressed = false;
	private bool _sCanPress = true, _hCanPress = true;

	private Light _light;
	[SerializeField]
	private Transform _capsule;
	private Material _material;

	public enum StationState { Passive, Triggerd, HalfDeactivated, Deactivated };
	private ObservedValue<StationState> _stationState = new ObservedValue<StationState>(StationState.Passive); 


	private ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }

	private void Awake() {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy() {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	public void Initialize() {
		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.SecurityStation.Creation(Id, transform.position.x, transform.position.z));
	}

	private void Start() {
		_light = GetComponentInChildren<Light>(true);
		ShooterAlarmManager.Instance.OnAlarmChange += onAlarmChange;
		_stationState.OnValueChange += OnStateChange;
	}

	public static void ProcessPacket(NetworkPacket.GameObjects.SecurityStation.hUpdate pPacket) {
		ShooterSecurityStation station = ShooterPackageSender.GetNetworkedObject<ShooterSecurityStation>(pPacket.Id);

		if (station != null) {
			station.hackerInteract();
		}
	}

	private void hackerInteract() {
		if (_hCanPress) {
			if (_sPressed) {
				ShooterAlarmManager.Instance.AlarmIsOn = false;
				_stationState.Value = StationState.Deactivated;
				TimerManager.CreateTimer("Security return to passive", true).SetDuration(3f).AddCallback(() => _stationState.Value = StationState.Passive).Start();
			} else {
				//_hPressed = true;
				//_hCanPress = false;
				//TimerManager.CreateTimer("Security Station Cooldown H", true).SetDuration(_interactCooldown).AddCallback(() => _hCanPress = true).Start();
			}
		}
	}

	public void Interact() {
		if (_sCanPress) {
				_sPressed = true;
				_sCanPress = false;
				_stationState.Value = StationState.HalfDeactivated;
			//TimerManager.CreateTimer("Security Station Threshold S", true).SetDuration(_interactThreshold).AddCallback(() => _sPressed = false).Start();
			//TimerManager.CreateTimer("Security Station Cooldown S", true).SetDuration(_interactCooldown).AddCallback(() => _sCanPress = true).Start();
		}
	}

	private void onAlarmChange() {
		if (ShooterAlarmManager.Instance.AlarmIsOn) {
			_sCanPress = true;
			_light.enabled = true;
			_stationState.Value = StationState.Triggerd;
			DOTween.Sequence()
			.Append(_light.DOIntensity(_light.intensity + _addedIntesity, 0.3f))
			.Append(_light.DOIntensity(_light.intensity, 0.3f))
			.SetLoops(-1)
			.SetTarget(_light);
		} else {
			_light.DOKill();
			_light.enabled = false;
			//_sCanPress = true;
		}
	}

	private void OnStateChange()
	{
		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.SecurityStation.sUpdate(Id, (int)_stationState.Value));
		switch (_stationState.Value)
		{
			case ShooterSecurityStation.StationState.Passive: ChangeColor(Color.white); break;
			case ShooterSecurityStation.StationState.Triggerd: ChangeColor(Color.red); break;
			case ShooterSecurityStation.StationState.HalfDeactivated: ChangeColor(Color.yellow); break;
			case ShooterSecurityStation.StationState.Deactivated: ChangeColor(Color.green); break;
		}

	}

	private void ChangeColor(Color color)
	{
		if (!_material)
		{
			_material = _capsule.GetComponent<Renderer>().material;
		}
		_material.color = color;
		ChangeLightColor(color);
	}

	private void ChangeLightColor(Color color)
	{
		_light.color = color;
	}
}
