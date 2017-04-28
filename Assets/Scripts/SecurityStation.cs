using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class SecurityStation : MonoBehaviour {
	//TODO Make Networked
	private void Start() {
		ShooterAlarmManager.Instance.OnAlarmChange += onAlarmChange;
	}

	private void onAlarmChange() {
		if (ShooterAlarmManager.Instance.AlarmIsOn) {
			//TODO Enable this device
		} else {
			//TODO Disable this device
		}
	}
}
