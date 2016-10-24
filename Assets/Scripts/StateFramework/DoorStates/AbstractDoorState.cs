using UnityEngine;
using System;

namespace StateFramework {
	public class AbstractDoorState : AbstractState {
		protected StateMachine<AbstractDoorState> _fsm = null;
		protected Door _door;

		public AbstractDoorState (Door pDoor, StateMachine<AbstractDoorState> pFsm) {
			_door = pDoor;
			_fsm = pFsm;
		}

		public override void Enter () { }
		public override void Step () { }
		public override void Exit () { }

		public virtual void Interact() { }
		public virtual void Lock () { }
	}
}