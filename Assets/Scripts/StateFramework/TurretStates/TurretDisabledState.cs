using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace StateFramework {
	public class TurretDisabledState : AbstractTurretState {
		//private Sequence _sequence;
		private float _disableTimer;
		private Sequence _sequence;

		public TurretDisabledState (ShooterTurret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) { }

		public override void Enter () {
			_turret.GetComponentInChildren<Light>().intensity = 0.0f;

			_sequence = DOTween.Sequence();
			_sequence.Append(_turret.RotaryBase.DOLocalMove(new Vector3(0.0f, 0.1f, 0.0f), _turret.Parameters.DeployDuration).SetEase(Ease.Linear));
			_sequence.Join(_turret.Gun.DOLocalRotate(new Vector3(-90.0f, 0.0f, 0.0f), _turret.Parameters.DeployDuration).SetEase(Ease.Linear));

			_turret.SeesTarget = false;

			_turret.GetComponentInChildren<Light>().intensity = 0.0f;

			_disableTimer = 0.0f;

			//_sequence = DOTween.Sequence();
			//_sequence.Append(_turret.RotaryBase.DOLocalMove(new Vector3(0.0f, 0.1f, 0.0f), _turret.DeployTime).SetEase(Ease.Linear));
			//_sequence.Join(_turret.Gun.DOLocalRotate(new Vector3(-90.0f, 0.0f, 0.0f), _turret.DeployTime).SetEase(Ease.Linear));

			_turret.SeesTarget = false;
		}

		public override void Step () {
			_disableTimer += Time.deltaTime;

			if (_disableTimer >= _turret.Parameters.DisableDuration) {
				_fsm.SetState<TurretGuardState>();
			}
		}

		public override void Exit () {
			_sequence.Kill();
		}
	}
}