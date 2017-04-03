using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public partial class TimerManager : MonoBehaviour {
	private static TimerManager _instance;
	private static Func<string, bool, Timer> TimerFactory;

	private static TimerManager Instance {
		get {
			if (_instance == null) {
				GameObject singleton = new GameObject("(singleton) TimerManager");
				_instance = singleton.AddComponent<TimerManager>();
				DontDestroyOnLoad(singleton);
			}

			return _instance;
		}
	}

	static TimerManager () {
		InternalTimer.Initialize();
	}

	private TimerManager () { }

	public static Timer CreateTimer (string pName, bool pKilledOnFinish) {
		Timer t = TimerFactory(pName, pKilledOnFinish);
		return t;
	}


	private Dictionary<Timer, Action> _timers = new Dictionary<Timer, Action>();

	public List<Timer> Timers {
		get {
			return _timers.Keys.ToList();
		}
	}

	private void Awake () {
		if (_instance != null) {
			Destroy(this.gameObject);
		}
	}

	private void OnDestroy () {
		foreach (KeyValuePair<Timer, Action> t in _timers) {
			t.Key.Kill();
		}
	}

	private void Update () {
		foreach (KeyValuePair<Timer, Action> t in _timers) {
			t.Value();
		}

		foreach (Timer t in _timers.Keys.ToArray()) {
			if (t.IsKilled)
				_timers.Remove(t);
		}
	}
}
