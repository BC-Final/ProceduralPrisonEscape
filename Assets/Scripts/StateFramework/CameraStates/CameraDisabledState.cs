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
			Timers.CreateTimer("Camera disable").SetTime(_camera.Parameters.DisableDuration).SetCallback(() => _fsm.SetState<CameraGuardState>()).Start();
		}

		public override void Step () { }

		public override void Exit () {
			_camera.GetComponentInChildren<Light>().intensity = _prevIntensity;
		}
	}
}