using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class HackerAlarmManager : Singleton<HackerAlarmManager> {
	private bool _alarmIsOn;

	public void ProcessPacket(NetworkPacket.States.AlarmState pPacket) {
		if (_alarmIsOn != pPacket.AlarmIsOn) {
			if (pPacket.AlarmIsOn) {
				EnableAlarm();
			} else {
				DisableAlarm();
			}
		}	
	}

	private void EnableAlarm() {
		_alarmIsOn = true;
	}

	private void DisableAlarm() {
		_alarmIsOn = false;
	}
}
