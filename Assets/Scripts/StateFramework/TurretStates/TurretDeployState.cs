using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace StateFramework {
	public class TurretDeployState : AbstractTurretState{
		private Sequence _sequence;

		public TurretDeployState (ShooterTurret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) { }

		public override void Enter () {
			_sequence = DOTween.Sequence();
			_sequence.Append(_turret.RotaryBase.DOLocalMove(new Vector3(0.0f, 0.8f, 0.0f), _turret.Parameters.DeployDuration).SetEase(Ease.Linear));
			_sequence.Join(_turret.Gun.DOLocalRotate(new Vector3(0.0f, 0.0f, 0.0f), _turret.Parameters.DeployDuration).SetEase(Ease.Linear));

			_turret.SeesTarget = true;
		}

		public override void Step () {
			if (!_sequence.IsPlaying()) {
				GameObject target = Utilities.AI.GetClosestObjectInView(_turret.transform, _turret.PossibleTargets, _turret.Parameters.ViewRange, 360.0f);

				if (target != null) {
					_fsm.SetState<TurretEngageState>();
				} else {
					_fsm.SetState<TurretScanState>();
				}

				//if (Vector3.Distance(_turret.transform.position, _player.transform.position) > _turret.SeeRange && !_turret.Controlled || _turret.Targets.Length > 0 && _turret.Controlled) {
				//	_fsm.SetState<TurretEngageState>();
				//} else {
				//	_fsm.SetState<TurretScanState>();
				//}
			}
		}

		public override void Exit () {
			_turret.GetComponentInChildren<Light>().intensity = 1.5f;
		}
	}
}