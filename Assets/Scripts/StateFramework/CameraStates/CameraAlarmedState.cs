using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class CameraAlarmedState : AbstractCameraState {
		private GameObject _player;

		public CameraAlarmedState (SecurityCamera pCamera, StateMachine<AbstractCameraState> pFsm) : base(pCamera, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
		}

		public override void Enter () {
			ShooterGamestateManager.Instance.TriggerAlarm();

			_camera.GetComponentInChildren<Light>().color = Color.red;
		}

		public override void Step () {
			rotateTowards(_player.transform);

			if (!canSeeObject(_player, _camera.LookPoint, _camera.SeeRange, 360.0f)) {
				_fsm.SetState<CameraGuardState>();
			}
		}

		public override void Exit () {
		}
	}
}