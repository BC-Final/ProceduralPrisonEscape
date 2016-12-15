using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class TurretIdleState : AbstractTurretState {
		private GameObject _player;

		public TurretIdleState (Turret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
		}

		public override void Enter () {
		}

		public override void Step () {
			if (Vector3.Distance(_turret.transform.position, _player.transform.position) < _turret.QuitIdleRange) {
				_fsm.SetState<TurretGuardState>();
			}
		}

		public override void Exit () {

		}
	}
}