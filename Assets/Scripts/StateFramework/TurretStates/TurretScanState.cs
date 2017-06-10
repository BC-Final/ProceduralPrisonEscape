﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace StateFramework {
	public class TurretScanState : AbstractTurretState {
		private float _scanTimer;

		//private Sequence _sequence;

		private float _currentLerpTime;

		private float _lerpTime;

		private int _currentDirection;

		private Vector3 _start;
		private Vector3 _end;

		public TurretScanState (ShooterTurret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) { }

		public override void Enter () {
			_currentDirection = 1;
			_currentLerpTime = 0.0f;

			_start = _turret.RotaryBase.localRotation.eulerAngles;
			_end = _start + new Vector3(0f, _turret.Parameters.ScanRotationAngle / 2f, 0f);

			_turret.GetComponentInChildren<Light>().color = Color.yellow;

			_scanTimer = 0.0f;

			_lerpTime = Quaternion.Angle(Quaternion.Euler(_start), Quaternion.Euler(_end)) / _turret.Parameters.ScanRotationSpeed;

			_turret.SeesTarget = false;

			//TODO Replace with lerp
			//_sequence.Append(_turret.Gun.DORotate(new Vector3(0.0f, 0.0f, 0.0f), 0.25f));
			//_sequence = DOTween.Sequence();
			//_sequence.Append(_turret.RotaryBase.DOBlendableLocalRotateBy(new Vector3(0.0f, 60.0f, 0.0f), 1.0f).SetEase(Ease.Linear));
			//_sequence.Append(_turret.RotaryBase.DOBlendableLocalRotateBy(new Vector3(0.0f, -120.0f, 0.0f), 2.0f).SetEase(Ease.Linear));
			//_sequence.Append(_turret.RotaryBase.DOBlendableLocalRotateBy(new Vector3(0.0f, 60.0f, 0.0f), 1.0f).SetEase(Ease.Linear));
			//_sequence.SetLoops(-1);
		}

		public override void Step () {
			GameObject target = Utilities.AI.GetClosestObjectInView(_turret.transform, _turret.PossibleTargets, _turret.Parameters.ViewRange, 360.0f);

			if (target != null) {
				_fsm.SetState<TurretEngageState>();
			}

			_scanTimer += Time.deltaTime;

			if (_scanTimer >= _turret.Parameters.ScanDuration) {
				_fsm.SetState<TurretHideState>();
			}

			_currentLerpTime += Time.deltaTime;
			if (_currentLerpTime > _lerpTime) {
				_currentLerpTime = _lerpTime;
			}

			float perc = _currentLerpTime / _lerpTime;
			_turret.RotaryBase.localRotation = Quaternion.Slerp(Quaternion.Euler(_start), Quaternion.Euler(_end), perc);

			if (_currentLerpTime == _lerpTime) {
				_currentLerpTime = 0.0f;
				_start = _end;

				_currentDirection = -_currentDirection;
				_end = _start + (new Vector3(0f, _turret.Parameters.ScanRotationAngle, 0f) * _currentDirection);

				_lerpTime = Quaternion.Angle(Quaternion.Euler(_start), Quaternion.Euler(_end)) / _turret.Parameters.ScanRotationSpeed;
			}
		}

		public override void Exit () { }
	}
}