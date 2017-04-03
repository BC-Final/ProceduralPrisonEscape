using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

partial class TimerManager {
	public class InternalTimer {
		internal static void Initialize () {
			TimerManager.TimerFactory = CreateTimer;
		}

		private static Timer CreateTimer (string pName, bool pKilledOnFinish) {
			InternalTimer it = new InternalTimer(pName, pKilledOnFinish);
			Timer t = new Timer(it, out it._killAction);

			TimerManager.Instance._timers.Add(t, it.step);

			return t;
		}

		private InternalTimer (string pName, bool pKilledOnFinish) {
			_name = pName;
			_killOnFinish = pKilledOnFinish;
		}


		private System.Action _killAction;

		private string _name = "Unnamed Timer";

		private float _duration = 0.0f;
		private float _currentTime = 0.0f;

		private float _minDuration = 0.0f;
		private float _maxDuration = 0.0f;

		private int _loopCount = 0;
		private int _currentLoop = 0;

		private Dictionary<System.Action, bool> _callbacks = new Dictionary<System.Action, bool>();

		private bool _playing = false;
		private bool _killed = false;
		private bool _finished = false;

		private bool _killOnFinish = false;
		private bool _resetOnFinish = false;
		private bool _useRandomDuration = false;
		private bool _useScaledDeltaTime = false;
		private bool _changeRandomTimeAfterLoop = true;



		public string Name { get { return _name; } }
		public float Duration { get { return _duration; } }
		public float MinDuration { get { return _minDuration; } }
		public float MaxDuration { get { return _maxDuration; } }
		public float CurrentTime { get { return _currentTime; } }
		public float RemainingTime { get { return _duration - _currentTime; } }
		public float FinishedPercentage { get { return Mathf.Clamp01(_currentTime / _duration); } }
		public float LoopCount { get { return _loopCount; } }
		public float CurrentLoop { get { return _currentLoop; } }
		public bool IsPlaying { get { return _playing; } }
		public bool IsFinished { get { return _finished; } }
		public bool IsKilled { get { return _killed; } }
		public bool KilledOnFinish { get { return _killOnFinish; } }
		public bool ResetsOnFinish { get { return _resetOnFinish; } }
		public bool UsesRandomDuration { get { return _useRandomDuration; } }
		public bool UsesScaledDeltaTime { get { return _useScaledDeltaTime; } }
		public bool ChangeRandomTimeAfterLoop { get { return _changeRandomTimeAfterLoop; } }

		public Dictionary<string, bool> CallbackInfo { get { return _callbacks.ToDictionary(x => x.Key.Method.ReflectedType.FullName, x => x.Value); } }


		public void SetDuration (float pDuration) {
			_duration = pDuration;
		}

		public void SetRandomDuration (float pMinDuration, float pMaxDuration, bool pChangeAfterLoop = true) {
			_minDuration = pMinDuration;
			_maxDuration = pMaxDuration;
			_useRandomDuration = true;
			_changeRandomTimeAfterLoop = pChangeAfterLoop;
			setRandomDuration();
		}

		public void SetLoops (int pLoopCount) {
			_loopCount = pLoopCount;
		}

		public void AddCallback (System.Action pAction, bool pOnlyOnFinish = false) {
			if (!_callbacks.ContainsKey(pAction)) {
				_callbacks.Add(pAction, pOnlyOnFinish);
			} else {
				Debug.LogError("Callback already exists!");
			}
		}

		public void ResetOnFinish (bool pResetOnFinish = true) {
			_resetOnFinish = pResetOnFinish;
		}

		public void UseScaledDeltaTime (bool pUseScaledDeltaTime = true) {
			_useScaledDeltaTime = pUseScaledDeltaTime;
		}



		public void Start () {
			_finished = false;
			_playing = true;
		}

		public void Pause (bool pSetFinished = false) {
			if (pSetFinished)
				_finished = true;

			_playing = false;
		}

		public void Stop () {
			Pause();
			Reset();
		}

		public void Reset (bool pAlsoResetLoops = true) {
			if (pAlsoResetLoops) {
				_currentLoop = 0;
			}

			_finished = false;

			_currentTime = 0.0f;
		}

		public void Kill (bool pInvokeCallbacks = false) {
			_finished = true;

			if (pInvokeCallbacks) {
				invokeCallbacks();
			}

			kill();
		}


		private void invokeCallbacks () {
			foreach (KeyValuePair<System.Action, bool> c in _callbacks) {
				if (_finished || _finished == c.Value) {
					c.Key();
				}
			}
		}

		private void kill () {
			_killAction();
			_killAction = null;
			_killed = true;
		}

		private void setRandomDuration() {
			_duration = Random.Range(_minDuration, _maxDuration);
		}



		private void step () {
			if (_playing) {
				_currentTime += _useScaledDeltaTime ? Time.deltaTime : Time.unscaledDeltaTime;

				while (_currentTime >= _duration) {
					_currentLoop++;

					invokeCallbacks();

					if (_loopCount < _currentLoop && _loopCount >= 0) {
						_currentTime = _resetOnFinish ? 0.0f : _duration;
						_currentLoop = _resetOnFinish ? 0 : _currentLoop;
						_playing = false;
						_finished = true;

						if (_useRandomDuration && !_changeRandomTimeAfterLoop) {
							setRandomDuration();
						}

						invokeCallbacks();

						if (_killOnFinish) {
							kill();
						}

						break;
					} else {
						_currentTime -= _duration;

						if (_useRandomDuration && _changeRandomTimeAfterLoop) {
							setRandomDuration();
						}
					}
				}
			}
		}
	}
}
