using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class TurretEngageState : AbstractTurretState {
		private GameObject _player;

		public TurretEngageState (Turret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
		}

		public override void Enter () {
			Debug.Log("Engage");
		}

		public override void Step () {
			rotateTowards(_player.transform);

			if (Vector3.Distance(_turret.transform.position, _player.transform.position) > _turret.SeeRange) {
				_fsm.SetState<TurretScanState>();
			}

			if (canSeeObject(_player, _turret.ShootPos, _turret.AttackRange, _turret.AttackAngle)) {
				_fsm.SetState<TurretAttackState>();
			}
		}

		public override void Exit () {

		}
	}
}
