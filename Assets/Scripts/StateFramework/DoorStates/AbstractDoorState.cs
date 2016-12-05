using UnityEngine;
using System;

namespace StateFramework {
	public class AbstractDoorState : AbstractState {
		protected StateMachine<AbstractDoorState> _fsm = null;
		protected ShooterDoor _door;

		public DoorState AssociatedState;

		public AbstractDoorState (ShooterDoor pDoor, StateMachine<AbstractDoorState> pFsm) {
			_door = pDoor;
			_fsm = pFsm;

			AssociatedState = DoorState.None;
		}

		public override void Enter () { }
		public override void Step () { }
		public override void Exit () { }

		public virtual void Interact() { }
		public virtual void Lock () { }
	}
}