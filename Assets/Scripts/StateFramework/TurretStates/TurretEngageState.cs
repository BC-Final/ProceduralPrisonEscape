using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class TurretEngageState : AbstractTurretState {
		public TurretEngageState (ShooterTurret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) { }

		public override void Enter () {
			_turret.GetComponentInChildren<Light>().intensity = 1.5f;
			_turret.GetComponentInChildren<Light>().color = Color.red;

			_turret.SeesTarget = true;
		}

		public override void Step () {
			GameObject target = Utilities.AI.GetClosestObjectInView(_turret.transform, _turret.PossibleTargets, _turret.Parameters.ViewRange, 360.0f);

			if (target != null) {
				rotateTowards(target.transform);

				if (Utilities.AI.ObjectInView(_turret.ShootPos, target.transform, _turret.Parameters.AttackRange, _turret.Parameters.AttackAngle)) {
					_fsm.SetState<TurretAttackState>();
				}
			} else {
				_fsm.SetState<TurretScanState>();
			}

			//rotateTowards(_player.transform);

			//if (_turret.Controlled) {
			//	if (getClosestSeeableObject(_turret.Targets, _turret.transform, _turret.SeeRange, 360.0f) == null) {
			//		_fsm.SetState<TurretScanState>();
			//	}

			//	if (getClosestSeeableObject(_turret.Targets, _turret.ShootPos, _turret.AttackRange, _turret.AttackAngle) != null) {
			//		_fsm.SetState<TurretAttackState>();
			//	}
			//} else {
			//	if (!canSeeObject(_player, _turret.transform, _turret.SeeRange, 360.0f)) {
			//		_fsm.SetState<TurretScanState>();
			//	}

			//	if (canSeeObject(_player, _turret.ShootPos, _turret.AttackRange, _turret.AttackAngle)) {
			//		_fsm.SetState<TurretAttackState>();
			//	}
			//}
		}

		public override void Exit () { }
	}
}
