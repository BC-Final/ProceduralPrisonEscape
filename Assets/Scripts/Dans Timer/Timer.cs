using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer {
	private TimerManager.InternalTimer _internalTimer;

	public Timer (TimerManager.InternalTimer pTimer, out System.Action pKillAction) {
		_internalTimer = pTimer;
		pKillAction = deleteTimer;
	}

	private void deleteTimer () {
		_internalTimer = null;
	}

	public string Name { get { return _internalTimer != null ? _internalTimer.Name : ""; } }
	public float Duration { get { return _internalTimer != null ? _internalTimer.Duration : 0.0f; } }
	public float MinDuration { get { return _internalTimer != null ? _internalTimer.MinDuration : 0.0f; } }
	public float MaxDuration { get { return _internalTimer != null ? _internalTimer.MaxDuration : 0.0f; } }
	public float CurrentTime { get { return _internalTimer != null ? _internalTimer.CurrentTime : 0.0f; } }
	public float RemainingTime { get { return _internalTimer != null ? _internalTimer.RemainingTime : 0.0f; } }
	public float FinishedPercentage { get { return _internalTimer != null ? _internalTimer.FinishedPercentage : 0.0f; } }
	public float LoopCount { get { return _internalTimer != null ? _internalTimer.LoopCount : 0; } }
	public float CurrentLoop { get { return _internalTimer != null ? _internalTimer.CurrentLoop : 0; } }
	public bool IsPlaying { get { return _internalTimer != null ? _internalTimer.IsPlaying : false; } }
	public bool IsFinished { get { return _internalTimer != null ? _internalTimer.IsFinished : false; } }
	public bool ResetsOnFinish { get { return _internalTimer != null ? _internalTimer.ResetsOnFinish : false; } }
	public bool KilledOnFinish { get { return _internalTimer != null ? _internalTimer.KilledOnFinish : false; } }
	public bool UsesRandomDuration { get { return _internalTimer != null ? _internalTimer.UsesRandomDuration : false; } }
	public bool UsesScaledDeltaTime { get { return _internalTimer != null ? _internalTimer.UsesScaledDeltaTime : false; } }
	public bool ChangeRandomTimeAfterLoop { get { return _internalTimer != null ? _internalTimer.ChangeRandomTimeAfterLoop : true; } }
	public bool IsKilled { get { return _internalTimer == null; } }

	public Dictionary<string, bool> CallbackInfo { get { return _internalTimer != null ? _internalTimer.CallbackInfo : null; } }



	public Timer SetDuration (float pDuration) {
		if (_internalTimer != null) {
			_internalTimer.SetDuration(pDuration);
			return this;
		} else {
			return null;
		}
	}

	public Timer SetRandomDuration (float pMinDuration, float pMaxDuration, bool pChangeAfterLoop = true) {
		if (_internalTimer != null) {
			_internalTimer.SetRandomDuration(pMinDuration, pMaxDuration, pChangeAfterLoop);
			return this;
		} else {
			return null;
		}
	}

	public Timer SetLoops (int pLoopCount) {
		if (_internalTimer != null) {
			_internalTimer.SetLoops(pLoopCount);
			return this;
		} else {
			return null;
		}
	}

	public Timer AddCallback (System.Action pAction, bool pOnlyOnFinish = false) {
		if (_internalTimer != null) {
			_internalTimer.AddCallback(pAction, pOnlyOnFinish);
			return this;
		} else {
			return null;
		}
	}

	public Timer ResetOnFinish (bool pResetOnFinish = true) {
		if (_internalTimer != null) {
			_internalTimer.ResetOnFinish(pResetOnFinish);
			return this;
		} else {
			return null;
		}
	}

	public Timer UseScaledDeltaTime (bool pUseScaledDeltaTime = true) {
		if (_internalTimer != null) {
			_internalTimer.UseScaledDeltaTime(pUseScaledDeltaTime);
			return this;
		} else {
			return null;
		}
	}



	public Timer Start () {
		if (_internalTimer != null) {
			_internalTimer.Start();
			return this;
		} else {
			return null;
		}
	}

	public Timer Pause () {
		if (_internalTimer != null) {
			_internalTimer.Pause();
			return this;
		} else {
			return null;
		}
	}

	public Timer Stop () {
		if (_internalTimer != null) {
			_internalTimer.Stop();
			return this;
		} else {
			return null;
		}
	}

	public Timer Reset (bool pAlsoResetLoops = true) {
		if (_internalTimer != null) {
			_internalTimer.Reset(pAlsoResetLoops);
			return this;
		} else {
			return null;
		}
	}

	public void Kill (bool pInvokeCallbacks = false) {
		if (_internalTimer != null) {
			_internalTimer.Kill(pInvokeCallbacks);
		}
	}
}
