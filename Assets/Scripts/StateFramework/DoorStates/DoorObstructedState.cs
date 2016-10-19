using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DoorObstructedState : AbstractDoorState {
		public DoorObstructedState (Door pDoor, StateMachine<AbstractDoorState> pFsm) : base(pDoor, pFsm) { }
		public override void Enter () { }
		public override void Step () { }
		public override void Exit () { }
	}
}