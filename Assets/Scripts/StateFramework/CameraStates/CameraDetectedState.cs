using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class CameraDetectState : AbstractCameraState {
		private GameObject _player;

		private FMOD.Studio.EventInstance _detectSound;
		private FMOD.Studio.EventInstance _loseSound;

		public CameraDetectState (ShooterCamera pCamera, StateMachine<AbstractCameraState> pFsm) : base(pCamera, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");


		}

		public override void Enter () {
			_detectSound = FMODUnity.RuntimeManager.CreateInstance("event:/PE_shooter/PE_shooter_camcatch");
			FMODUnity.RuntimeManager.AttachInstanceToGameObject(_detectSound, _camera.transform, _camera.GetComponent<Rigidbody>());
			_detectSound.start();

			_camera.GetComponentInChildren<Light>().color = Color.yellow;
		}

		public override void Step () {
			rotateTowards(_player.transform);

			if (!canSeeObject(_player, _camera.LookPoint, _camera.SeeRange, 360.0f)) {
				_fsm.SetState<CameraGuardState>();
			}
		}

		public override void Exit () {
			_loseSound = FMODUnity.RuntimeManager.CreateInstance("event:/PE_shooter/PE_shooter_camlost");
			FMODUnity.RuntimeManager.AttachInstanceToGameObject(_loseSound, _camera.transform, _camera.GetComponent<Rigidbody>());
			_loseSound.start();
		}
	}
}