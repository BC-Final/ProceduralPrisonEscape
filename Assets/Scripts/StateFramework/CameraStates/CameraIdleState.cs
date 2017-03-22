using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class CameraIdleState : AbstractCameraState {
		//private GameObject _player;

		public CameraIdleState (ShooterCamera pCamera, StateMachine<AbstractCameraState> pFsm) : base(pCamera, pFsm) {
			//_player = GameObject.FindGameObjectWithTag("Player");
		}

		public override void Enter () {
			
		}

		public override void Step () {
			//if (Vector3.Distance(_camera.transform.position, _player.transform.position) < _camera.QuitIdleRange) {
			//	_fsm.SetState<CameraGuardState>();
			//}
		}

		public override void Exit () {

		}
	}
}