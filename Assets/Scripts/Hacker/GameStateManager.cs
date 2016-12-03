using UnityEngine;
using System.Collections;
using Gamelogic.Extensions;

public class GameStateManager : Singleton<GameStateManager> {
	[SerializeField]
	private int _maxSuspicion;
	private int _currentSuspicion;

	private bool _alarm;
	public bool Alarm {
		get { return _alarm; }
		set { _alarm = value; }
	}

	//Suggestion Should this be in the Security node?
	public void IncreaseSuspicion() {
		_currentSuspicion++;

		if (_currentSuspicion >= _maxSuspicion) {
			//TODO Send Admin to investigsate
		}
	}

	public void DecreaseSuspicion() {
		_currentSuspicion--;
	}
}
