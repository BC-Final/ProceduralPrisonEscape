using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class TurretGuardState : AbstractTurretState {
		private GameObject _player;

		public TurretGuardState (Turret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
		}

		public override void Enter () {
			_turret.SeesTarget = false;
		}

		public override void Step () {
			//TODO Rotate

			if (canSeeObject(_player, _turret.transform, _turret.SeeRange, 360.0f) || _turret.Controlled) {
				_fsm.SetState<TurretDeployState>();
			}

			if (Vector3.Distance(_turret.transform.position, _player.transform.position) > _turret.QuitIdleRange) {
				_fsm.SetState<TurretIdleState>();
			}
		}

		public override void Exit () {

		}

		public override void ReceiveDamage (Vector3 pDirection, Vector3 pPoint, float pDamage) {
			_fsm.SetState<TurretDeployState>();
		}
	}
}