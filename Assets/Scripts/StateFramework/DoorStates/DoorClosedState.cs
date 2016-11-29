using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace StateFramework {
	public class DoorClosedState : AbstractDoorState {
		public DoorClosedState (ShooterDoor pDoor, StateMachine<AbstractDoorState> pFsm) : base(pDoor, pFsm) { }


		public override void Enter () {
			_door.SetDoorState(DoorState.Closed);

			_door.RightDoor.DOLocalMove(new Vector3(0.625f, 1.25f, 0.0f), 1.0f);
			_door.LeftDoor.DOLocalMove(new Vector3(-0.625f, 1.25f, 0.0f), 1.0f);

			_door.RightDoor.DOScale(new Vector3(1.25f, 2.5f, 0.5f), 1.0f);
			_door.LeftDoor.DOScale(new Vector3(1.25f, 2.5f, 0.5f), 1.0f);
		}

		public override void Step () { }

		public override void Exit () { }

		public override void Interact () {
			if (_door.GetFireWall() == null || _door.GetFireWall().GetState()) {
				if (_door._requireKeyCard && GameObject.FindObjectOfType<Inventory>().Contains(_door) || !_door._requireKeyCard) {
					_fsm.SetState<DoorOpenState>();
				}
			}
		}
	}
}