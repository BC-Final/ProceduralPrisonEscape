using UnityEngine;
using System.Collections;
using Gamelogic.Extensions;

public class GameStateManager : Singleton<GameStateManager> {
	[SerializeField]
	private int _maxSuspicion;
	private int _currentSuspicion;

	private bool _alarm;


	public void TriggerAlarm () {
		_alarm = true;
		_currentSuspicion = 0;
		HackerPackageSender.SendPackage(new CustomCommands.Update.AlarmUpdate(true));
	}

	public void DisableAlarm () {
		Debug.Log("Disable Alarm");
		_alarm = false;
		HackerPackageSender.SendPackage(new CustomCommands.Update.AlarmUpdate(false));
	}

	//Suggestion Should this be in the Security node?
	public void IncreaseSuspicion() {
		_currentSuspicion++;

		if (_currentSuspicion >= _maxSuspicion) {
			//TODO Send Admin to investigsate
			TriggerAlarm();
		}
	}

	public void DecreaseSuspicion() {
		_currentSuspicion--;
	}
}
