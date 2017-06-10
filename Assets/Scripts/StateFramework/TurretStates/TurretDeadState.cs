using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class TurretDeadState : AbstractTurretState {
		public TurretDeadState (ShooterTurret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) {
			
		}

		public override void Enter () {
			//Debug.Log("Dead");
			//TODO Play death animation
		}

		public override void Step () {

		}

		public override void Exit () {

		}
	}
}