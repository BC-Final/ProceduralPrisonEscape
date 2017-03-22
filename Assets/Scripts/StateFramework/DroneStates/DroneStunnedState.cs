using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneStunnedState : AbstractDroneState {
		private float _stunEndTime;

		public DroneStunnedState (DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) { }

		public override void Enter() {
			_drone.SeesTarget = false;
			_stunEndTime = Time.time + _drone.Parameters.StunDuration;
		}

		public override void Step() {
			if (_stunEndTime - Time.time <= 0.0f) {
				//TODO Engage State might be the wrong state to be in after stunned
				_fsm.SetState<DroneEngangeState>();
			}
		}

		public override void Exit() { }
	}
}