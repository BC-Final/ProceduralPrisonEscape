using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class CameraDisabledState : AbstractCameraState {
		public CameraDisabledState (ShooterCamera pCamera, StateMachine<AbstractCameraState> pFsm) : base(pCamera, pFsm) { }

		float _prevIntensity;

		public override void Enter () {
			_prevIntensity = _camera.GetComponentInChildren<Light>().intensity;
			_camera.GetComponentInChildren<Light>().intensity = 0.0f;
			TimerManager.CreateTimer("Camera disable", true).SetDuration(_camera.Parameters.DisableDuration).AddCallback(() => _fsm.SetState<CameraGuardState>()).Start();

			if (_camera._onDisable != null) {
				_camera._onDisable.Invoke();
			}
		}

		public override void Step () { }

		public override void Exit () {
			_camera.GetComponentInChildren<Light>().intensity = _prevIntensity;
			if (_camera._onEnable != null) {
				_camera._onEnable.Invoke();
			}
		}
	}
}