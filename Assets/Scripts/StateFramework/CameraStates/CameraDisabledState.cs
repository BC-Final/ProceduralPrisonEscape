using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class CameraDisabledState : AbstractCameraState {
		public CameraDisabledState (ShooterCamera pCamera, StateMachine<AbstractCameraState> pFsm) : base(pCamera, pFsm) {
			_disableTimer = Timers.CreateTimer().SetTime(pCamera.DisabledTime).SetCallback(() => _fsm.SetState<CameraGuardState>()).ResetOnFinish();
		}

		Timers.Timer _disableTimer;
		float _prevIntensity;

		public override void Enter () {
			_prevIntensity = _camera.GetComponentInChildren<Light>().intensity;
			_camera.GetComponentInChildren<Light>().intensity = 0.0f;
			_disableTimer.Start();
		}

		public override void Step () {

		}

		public override void Exit () {
			//ShooterPackageSender.SendPackage(new CustomCommands.Update.EnableCamera(_camera.Id));
			//_camera.GetComponentInChildren<Light>().intensity = _prevIntensity;
		}
	}
}