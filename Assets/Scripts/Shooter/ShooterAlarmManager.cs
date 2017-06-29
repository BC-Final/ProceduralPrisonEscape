using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;
using System.Linq;

public class ShooterAlarmManager : Singleton<ShooterAlarmManager> {
	private ObservedValue<bool> _alarmIsOn = new ObservedValue<bool>(false);

	[SerializeField]
	private UnityEngine.Events.UnityEvent _onAlarmOn;

	[SerializeField]
	private UnityEngine.Events.UnityEvent _onAlarmOff;

	private void Awake() {
		_alarmIsOn.OnValueChange += () => { if (AlarmIsOn && _onAlarmOn != null) { _onAlarmOn.Invoke(); } else if(!AlarmIsOn && _onAlarmOff != null) { _onAlarmOff.Invoke(); } };
	}

	public event System.Action OnAlarmChange {
		add { _alarmIsOn.OnValueChange += value; }
		remove { _alarmIsOn.OnValueChange -= value; }
	}

	public bool AlarmIsOn {
		get { return _alarmIsOn.Value; }
		set {
			_alarmIsOn.Value = value;
			ShooterPackageSender.SendPackage(new NetworkPacket.States.AlarmState(AlarmIsOn));
		}
	}
}
