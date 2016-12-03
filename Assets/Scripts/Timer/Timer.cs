using UnityEngine;
using System.Collections;
using System;

partial class Timers {
	public class Timer {
		private float _time;

		private int _loopCount;
		private bool _useRealTime;
		private bool _destroyOnFinish;

		private Action _callback;

		private bool _playing;
		private bool _killed;

		private float _currentTime;
		private int _currentloopCount;



		internal Timer () {
			_time = 0.0f;

			_loopCount = 0;

			_playing = false;
			_killed = false;
			_useRealTime = false;
			_destroyOnFinish = false;

			_currentTime = 0.0f;
			_currentloopCount = 0;

			Timers.Instance.RegisterTimer(this);
		}



		public Timer SetTime (float pTime) {
			_time = pTime;
			return this;
		}

		public Timer SetLoop (int pCount) {
			_loopCount = pCount;
			return this;
		}

		public Timer UseRealTime (bool pUseRealTime) {
			_useRealTime = pUseRealTime;
			return this;
		}

		public Timer Play () {
			_playing = true;
			return this;
		}

		public Timer Pause () {
			_playing = false;
			return this;
		}

		public Timer DestroyOnFinish () {
			_destroyOnFinish = true;
			return this;
		}

		public Timer AddCallback (Action pCallback) {
			_callback = pCallback;
			return this;
		}



		public bool IsPlaying {
			get { return _playing; }
		}

		public int LoopCount {
			get { return _currentloopCount; }
		}

		public float CurrentTime {
			get { return _currentTime; }
		}

		public float RemainingTime {
			get { return _time - _currentTime; }
		}

		public bool IsKilled {
			get { return _killed; }
		}



		internal void Step () {
			if (_playing) {
				_currentTime += _useRealTime ? Time.unscaledDeltaTime : Time.deltaTime;

				if (_currentTime >= _time) {
					_currentloopCount++;
					_currentTime -= _time;

					_callback();

					if (_loopCount < _currentloopCount && _loopCount > 0) {
						_playing = false;
						_killed = true;

						if (_destroyOnFinish) {
							Timers.Instance.UnregisterTimer(this);
						}
					}
				}
			}
		}
	}
}