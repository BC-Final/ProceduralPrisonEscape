using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class PhaserMeleeState : AbstractWeaponState {
		protected StateMachine<AbstractWeaponState> _fsm = null;
		protected Weapon _weapon;

		public PhaserMeleeState(Weapon pWeapon, StateMachine<AbstractWeaponState> pFsm) : base(pWeapon, pFsm) {
			_weapon = pWeapon;
			_fsm = pFsm;
		}

		public override void Enter() { }
		public override void Step() { }
		public override void Exit() { }
	}
}