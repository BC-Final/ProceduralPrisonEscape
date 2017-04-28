using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace StateFramework {
	public class CameraGuardState : AbstractCameraState {
		private Vector3 _startRotation;

		private float _currentLerpTime;

		private Vector3 _leftPos;
		private Vector3 _rightPos;

		private Vector3 _start;
		private Vector3 _end;

		private float _lerpTime;

		private int _currentDirection;

		//private FMOD.Studio.EventInstance _moveSound;
		//private FMOD.Studio.EventInstance _moveStopSound;

		public CameraGuardState (ShooterCamera pCamera, StateMachine<AbstractCameraState> pFsm) : base(pCamera, pFsm) {
			_startRotation = _camera.Base.rotation.eulerAngles;

			_leftPos = _startRotation + new Vector3(0f, _camera.Parameters.RotationAngle / 2f, 0f);
			_rightPos = _startRotation + new Vector3(0f,  - _camera.Parameters.RotationAngle / 2f, 0f);
		}

		public override void Enter () {
			_currentLerpTime = 0.0f;

			_start = _camera.Base.rotation.eulerAngles;
			_end = _leftPos;

			_currentDirection = 1;

			_lerpTime = Quaternion.Angle(Quaternion.Euler(_start), Quaternion.Euler(_end)) / _camera.Parameters.RotationSpeed;

			_camera.GetComponentInChildren<Light>().color = Color.green;

			//_moveSound = FMODUnity.RuntimeManager.CreateInstance("event:/PE_hacker/PE_hacker_cameramove_start");
			//FMODUnity.RuntimeManager.AttachInstanceToGameObject(_moveSound, _camera.transform, _camera.GetComponent<Rigidbody>());

			//_moveSound.start();
		}

		public override void Step () {
			GameObject target = Utilities.AI.GetClosestObjectInView(_camera.LookPoint, _camera.PossibleTargets, _camera.Parameters.ViewRange, _camera.Parameters.ViewAngle);

			if (target != null) {
				if (target.GetComponent<PlayerHealth>() != null && _camera.Faction != Faction.Neutral) {
					_fsm.SetState<CameraDetectState>();
				} else if (_camera.Faction == Faction.Neutral) {
					//TODO Send Hacker Update info about enemies in view
				}
				
			}

			_currentLerpTime += Time.deltaTime;
			if (_currentLerpTime > _lerpTime) {
				_currentLerpTime = _lerpTime;
			}

			float perc = _currentLerpTime / _lerpTime;
			_camera.Base.rotation = Quaternion.Slerp(Quaternion.Euler(_start), Quaternion.Euler(_end), perc);

			if (_currentLerpTime == _lerpTime) {
				_currentLerpTime = 0.0f;
				_start = _end;

				_currentDirection = -_currentDirection;

				_lerpTime = _camera.Parameters.RotationAngle / _camera.Parameters.RotationSpeed;

				if (_currentDirection == -1) {
					_end = _rightPos;
				} else {
					_end = _leftPos;
				}

				//_moveStopSound = FMODUnity.RuntimeManager.CreateInstance("event:/PE_hacker/PE_hacker_cameramove_stop");
				//FMODUnity.RuntimeManager.AttachInstanceToGameObject(_moveStopSound, _camera.transform, _camera.GetComponent<Rigidbody>());
				//_moveStopSound.start();
			}
		}

		public override void Exit () {
			//_moveSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}
}