using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public partial class Timers : MonoBehaviour {
	private static Timers _instance;

	internal static Timers Instance {
		get {
			if (_instance == null) {
				GameObject singleton = new GameObject();
				_instance = singleton.AddComponent<Timers>();
				singleton.name = "(singleton) TimerManager";

				DontDestroyOnLoad(singleton);
			}

			return _instance;
		}
	}

	public static Timer CreateTimer () {
		return new Timer();
	}

	private List<Timer> _timers;

	private Timers () {
		_timers = new List<Timer>();
	}

	private void Update () {
		_timers.ForEach(x => x.Step());
	}

	internal void UnregisterTimer (Timer pTimer) {
		_timers.Remove(pTimer);
	}

	internal void RegisterTimer (Timer pTimer) {
		_timers.Add(pTimer);
	}
}
