using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace StateFramework {
	public class DoorClosedState : AbstractDoorState {
		public DoorClosedState (ShooterDoor pDoor, StateMachine<AbstractDoorState> pFsm) : base(pDoor, pFsm) {
		}

		public override void Enter () {
			//_door.SendStateUpdate(AssociatedState);

			//_door.RightDoor.DOLocalMove(new Vector3(0.625f, 1.25f, 0.0f), 1.0f).OnComplete(() => _door.ForceField.SetActive(false));
			//_door.LeftDoor.DOLocalMove(new Vector3(-0.625f, 1.25f, 0.0f), 1.0f);

			//_door.RightDoor.DOScale(new Vector3(1.25f, 2.5f, 0.5f), 1.0f);
			//_door.LeftDoor.DOScale(new Vector3(1.25f, 2.5f, 0.5f), 1.0f);
		}

		public override void Step () { }

		public override void Exit () { }

		public override void Interact () {
			//if ((_door.RequireKeycard && GameObject.FindObjectOfType<Inventory>().Contains(_door) || !_door.RequireKeycard) && _door.Openable) {
			//	_fsm.SetState<DoorOpenState>();

			//	FMOD.Studio.EventInstance ins = FMODUnity.RuntimeManager.CreateInstance("event:/PE_envi/PE_envi_door_access");
			//	FMODUnity.RuntimeManager.AttachInstanceToGameObject(ins, _door.transform, _door.GetComponent<Rigidbody>());
			//	ins.start();
				
			//} else {
			//	FMOD.Studio.EventInstance ins = FMODUnity.RuntimeManager.CreateInstance("event:/PE_hacker/PE_hacker_door_close");
			//	FMODUnity.RuntimeManager.AttachInstanceToGameObject(ins, _door.transform, _door.GetComponent<Rigidbody>());
			//	ins.start();
			//}
		}
	}
}