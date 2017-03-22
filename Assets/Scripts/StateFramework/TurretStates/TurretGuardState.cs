using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class TurretGuardState : AbstractTurretState {
		public TurretGuardState (Turret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) { }

		public override void Enter () {
			_turret.SeesTarget = false;
		}

		public override void Step () {
			GameObject target = Utilities.AI.GetClosestObjectInView(_turret.transform, _turret.PossibleTargets, _turret.Parameters.ViewRange, 360.0f);

			if (target != null) {
				_fsm.SetState<TurretDeployState>();
			}

			//if (canSeeObject(_player, _turret.transform, _turret.SeeRange, 360.0f) || _turret.Controlled) {
			//	_fsm.SetState<TurretDeployState>();
			//}

			//if (Vector3.Distance(_turret.transform.position, _player.transform.position) > _turret.QuitIdleRange) {
			//	_fsm.SetState<TurretIdleState>();
			//}
		}

		public override void Exit () { }

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