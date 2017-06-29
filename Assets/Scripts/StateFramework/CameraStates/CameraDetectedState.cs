using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class CameraDetectState : AbstractCameraState {
		//private FMOD.Studio.EventInstance _detectSound;
		//private FMOD.Studio.EventInstance _loseSound;

		private float _alarmTimer;

		public CameraDetectState (ShooterCamera pCamera, StateMachine<AbstractCameraState> pFsm) : base(pCamera, pFsm) { }

		public override void Enter () {
			//_detectSound = FMODUnity.RuntimeManager.CreateInstance("event:/PE_shooter/PE_shooter_camcatch");
			//FMODUnity.RuntimeManager.AttachInstanceToGameObject(_detectSound, _camera.transform, _camera.GetComponent<Rigidbody>());
			//_detectSound.start();

			_camera.GetComponentInChildren<Light>().color = Color.yellow;

			_alarmTimer = 0.0f;

			if (_camera._onDetect != null) {
				_camera._onDetect.Invoke();
			}
		}

		public override void Step () {
			GameObject target = Utilities.AI.GetClosestObjectInView(_camera.LookPoint.transform, _camera.PossibleTargets, _camera.Parameters.ViewRange, _camera.Parameters.ViewAngle);

			if (target != null) {
				rotateTowards(target.transform);

				_alarmTimer += Time.deltaTime;

				if (_alarmTimer >= _camera.Parameters.TriggerDelay) {
					ShooterAlarmManager.Instance.AlarmIsOn = true;
				}
			} else {
				_fsm.SetState<CameraGuardState>();
			}	
		}

		public override void Exit () {
			//_loseSound = FMODUnity.RuntimeManager.CreateInstance("event:/PE_shooter/PE_shooter_camlost");
			//FMODUnity.RuntimeManager.AttachInstanceToGameObject(_loseSound, _camera.transform, _camera.GetComponent<Rigidbody>());
			//_loseSound.start();

			if (_camera._onLoose != null) {
				_camera._onLoose.Invoke();
			}
		}
	}
}