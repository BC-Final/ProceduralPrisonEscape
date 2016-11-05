using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class PhaserIdleState : AbstractWeaponState {
		protected StateMachine<AbstractWeaponState> _fsm = null;
		protected Weapon _weapon;

		public PhaserIdleState(Weapon pWeapon, StateMachine<AbstractWeaponState> pFsm) : base (pWeapon, pFsm){
			_weapon = pWeapon;
			_fsm = pFsm;
		}

		public override void Enter() { }

		public override void Step() {
			if (Input.GetMouseButtonDown(0)) {
				_fsm.SetState<PhaserShootState>();
			}

			if (Input.GetKeyDown(KeyCode.R)) {
				_fsm.SetState<PhaserReloadState>();
			}

			if (Input.GetKeyDown(KeyCode.F)) {
				_fsm.SetState<PhaserMeleeState>();
			}
		}

		public override void Exit() { }
	}
}

