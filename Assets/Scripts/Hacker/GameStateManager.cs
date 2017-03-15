using UnityEngine;
using System.Collections;
using Gamelogic.Extensions;

public class GameStateManager : Singleton<GameStateManager> {
	//TODO Rewrite Alarm code!!
	[SerializeField]
	private int _maxSuspicion;
	private int _currentSuspicion;

	private bool _alarm;

	private FMOD.Studio.EventInstance _alarmLoop;

	private void Start () {
		_alarmLoop = FMODUnity.RuntimeManager.CreateInstance("event:/PE_hacker/PE_hacker_alarm_loop");
	}

	public void TriggerAlarm () {
		if (!_alarm) {
			_alarm = true;
			_currentSuspicion = 0;
			HackerPackageSender.SendPackage(new CustomCommands.Update.AlarmUpdate(true));

			_alarmLoop.start();
		}
	}

	public void DisableAlarm () {
		_alarm = false;
		_alarmLoop.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		HackerPackageSender.SendPackage(new CustomCommands.Update.AlarmUpdate(false));
	}

	//Suggestion Should this be in the Security node?
	public void IncreaseSuspicion() {
		if (!_alarm) {
			_currentSuspicion++;

			if (_currentSuspicion >= _maxSuspicion) {
				//TODO Send Admin to investigsate
				TriggerAlarm();
			}
		}
	}

	public void DecreaseSuspicion() {
		_currentSuspicion--;
	}
}
