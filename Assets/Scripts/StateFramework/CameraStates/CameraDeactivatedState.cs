using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class CameraDisabledState : AbstractCameraState {
		public CameraDisabledState (ShooterCamera pCamera, StateMachine<AbstractCameraState> pFsm) : base(pCamera, pFsm) {

		}

		public override void Enter () {
			//TODO Move this to seesplayer and send packet to hacker
			//AlarmManager.Instance.ActivateAlarm();

			_camera.GetComponentInChildren<Light>().color = Color.red;
		}

		public override void Step () {

		}

		public override void Exit () {
		}
	}
}