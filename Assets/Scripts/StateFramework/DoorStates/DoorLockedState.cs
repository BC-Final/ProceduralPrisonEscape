using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DoorLockedState : AbstractDoorState {
		public DoorLockedState (ShooterDoor pDoor, StateMachine<AbstractDoorState> pFsm) : base(pDoor, pFsm) { }
		public override void Enter () { }
		public override void Step () { }
		public override void Exit () { }
	}
}