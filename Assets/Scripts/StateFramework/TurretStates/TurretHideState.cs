using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace StateFramework {
	public class TurretHideState : AbstractTurretState {
		private Sequence _sequence;

		public TurretHideState (Turret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) { }

		public override void Enter () {
			_turret.GetComponentInChildren<Light>().intensity = 0.0f;

			_sequence = DOTween.Sequence();
			_sequence.Append(_turret.RotaryBase.DOLocalMove(new Vector3(0.0f, 0.1f, 0.0f), _turret.Parameters.DeployDuration).SetEase(Ease.Linear));
			_sequence.Join(_turret.Gun.DOLocalRotate(new Vector3(-90.0f, 0.0f, 0.0f), _turret.Parameters.DeployDuration).SetEase(Ease.Linear));

			_turret.SeesTarget = false;
		}

		public override void Step () {
			GameObject target = Utilities.AI.GetClosestObjectInView(_turret.transform, _turret.PossibleTargets, _turret.Parameters.ViewRange, 360.0f);

			if (target != null) {
				_fsm.SetState<TurretDeployState>();
			}

			if (!_sequence.IsPlaying()) {
				_fsm.SetState<TurretGuardState>();
			}
		}

		public override void Exit () {
			_sequence.Kill();
		}

		public override void ReceiveDamage (IDamageable pSender, Vector3 pDirection, Vector3 pPoint, float pDamage) {
			if (pSender == null || Utilities.AI.FactionIsEnemy(_turret.Faction, pSender.Faction)) {
				//if (pSender == null) {
				//	_turret.LastTarget = null;
				//} else {
				//	_turret.LastTarget = pSender.GameObject;
				//}

				_fsm.SetState<TurretDeployState>();
			}
		}
	}
}