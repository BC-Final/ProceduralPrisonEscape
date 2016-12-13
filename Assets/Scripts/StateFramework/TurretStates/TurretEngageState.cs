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

			if(!canSeeObject(_player, _turret.transform, _turret.SeeRange, 360.0f)) { 
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
