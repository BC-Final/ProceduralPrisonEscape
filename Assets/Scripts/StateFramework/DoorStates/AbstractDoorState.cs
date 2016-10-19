using UnityEngine;
using System;

namespace StateFramework {
	public class AbstractDoorState : AbstractState {
		protected Door _door;
		protected StateMachine<AbstractDoorState> _fsm = null;

		public AbstractDoorState (Door pDoor, StateMachine<AbstractDoorState> pFsm) {
			_door = pDoor;
			_fsm = pFsm;
		}

		public override void Enter () { }
		public override void Step () { }
		public override void Exit () { }
	}
}