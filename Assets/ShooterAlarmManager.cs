using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class ShooterAlarmManager : Singleton<ShooterAlarmManager> {
	private ObservedValue<bool> _alarmIsOn = new ObservedValue<bool>(false);

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

	private void Update() {
		//HACK This is for testing
		if (Input.GetKeyDown(KeyCode.Keypad0)) {
			AlarmIsOn = !AlarmIsOn;
			Debug.Log("Alarm: " + AlarmIsOn);
		}
	}
}
