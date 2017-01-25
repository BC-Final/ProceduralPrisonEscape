using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;
using DG.Tweening;

public class ShooterGamestateManager : Singleton<ShooterGamestateManager> {
	
	private bool _alarm;

	[SerializeField]
	private Light _alarmLight1;

   [SerializeField]
	private Light _alarmLight2;

	Sequence _seq;

	public void TriggerAlarm () {
		Debug.Log("Enabled lights");

		_alarm = true;

		_seq = DOTween.Sequence();
		_seq.Append(_alarmLight1.DOIntensity(0.5f, 0.5f));
		_seq.Join(_alarmLight2.DOIntensity(0.5f, 0.5f));
		_seq.Append(_alarmLight1.DOIntensity(0.0f, 0.5f));
		_seq.Join(_alarmLight2.DOIntensity(0.0f, 0.5f));
		_seq.SetLoops(-1);
	}

	public void DisableAlarm () {

		Debug.Log("Disabled lights");
		_alarm = false;

		_seq.Kill();

		_alarmLight1.intensity = 0.0f;
		_alarmLight1.intensity = 0.0f;
		//_alarmLight1.DOIntensity(0.0f, 0.5f);
		//_alarmLight2.DOIntensity(0.0f, 0.5f);
	}
}
