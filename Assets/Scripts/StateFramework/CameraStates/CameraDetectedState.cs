using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class CameraDetectState : AbstractCameraState {
		private GameObject _player;

		private float _detectTimer;

		public CameraDetectState (SecurityCamera pCamera, StateMachine<AbstractCameraState> pFsm) : base(pCamera, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
		}

		public override void Enter () {
			_camera.GetComponentInChildren<Light>().color = Color.yellow;

			_detectTimer = 0.0f;
		}

		public override void Step () {
			rotateTowards(_player.transform);

			_detectTimer += Time.deltaTime;

			if (_detectTimer > _camera.TriggerTime) {
				_fsm.SetState<CameraAlarmedState>();
			}

			if (!canSeeObject(_player, _camera.LookPoint, _camera.SeeRange, 360.0f)) {
				_fsm.SetState<CameraGuardState>();
			}
		}

		public override void Exit () {

		}
	}
}