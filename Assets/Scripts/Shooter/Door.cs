using UnityEngine;
using StateFramework;


public class Door : MonoBehaviour, IInteractable {
	private StateMachine<AbstractDoorState> _fsm = null;

	public Transform LeftDoor;
	public Transform RightDoor;

	private void Start () {
		_fsm = new StateMachine<AbstractDoorState>();

		_fsm.AddState(new DoorOpenState(this, _fsm));
		_fsm.AddState(new DoorClosedState(this, _fsm));
		_fsm.AddState(new DoorLockedState(this, _fsm));
		_fsm.AddState(new DoorProtectedState(this, _fsm));
		_fsm.AddState(new DoorObstructedState(this, _fsm));

		//TODO Determine Starting state
		_fsm.SetState<DoorClosedState>();
	}

	private void Update () {
		_fsm.Step();
	}

	public void Interact () {
		_fsm.GetState().Interact();
	}
}
