using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class PhaserShootState : AbstractWeaponState {
		private float _fireRateTimer;

		public PhaserShootState(Weapon pWeapon, StateMachine<AbstractWeaponState> pFsm) : base(pWeapon, pFsm) {
			_weapon = pWeapon;
			_fsm = pFsm;
		}

		public override void Enter() {
			_fireRateTimer = Time.time + _weapon.ShootRate;

			//FILL Shoot the gun
		}

		public override void Step() {
			if (_fireRateTimer - Time.time < 0.0f) {
				_fsm.SetState<PhaserIdleState>();
			}
		}

		public override void Exit() { }
	}
}