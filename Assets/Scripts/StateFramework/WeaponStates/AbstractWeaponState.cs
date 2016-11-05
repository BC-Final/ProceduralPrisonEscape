using UnityEngine;
using System.Collections;
namespace StateFramework {
	public class AbstractWeaponState : AbstractState {
		protected StateMachine<AbstractWeaponState> _fsm = null;
		protected Weapon _weapon;

		public AbstractWeaponState(Weapon pWeapon, StateMachine<AbstractWeaponState> pFsm) {
			_weapon = pWeapon;
			_fsm = pFsm;
		}

		public override void Enter() { }
		public override void Step() { }
		public override void Exit() { }
	}

}