using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[SelectionBase]
public class SecurityStation : MonoBehaviour {
	[SerializeField]
	private float _addedIntesity;

	private Light _light;

	//TODO Make Networked
	private void Start() {
		_light = GetComponentInChildren<Light>(true);
		ShooterAlarmManager.Instance.OnAlarmChange += onAlarmChange;
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
