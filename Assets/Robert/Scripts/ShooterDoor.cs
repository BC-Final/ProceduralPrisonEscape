using UnityEngine;
using System.Collections;
using StateFramework;

public class ShooterDoor : Door, IInteractable {
	private StateMachine<AbstractDoorState> _fsm = null;

	public Transform LeftDoor;
	public Transform RightDoor;

	public override void Start()
	{
		base.Start();
	
	_fsm = new StateMachine<AbstractDoorState>();

		_fsm.AddState(new DoorOpenState(this, _fsm));
		_fsm.AddState(new DoorClosedState(this, _fsm));
		_fsm.AddState(new DoorLockedState(this, _fsm));
		_fsm.AddState(new DoorProtectedState(this, _fsm));
		_fsm.AddState(new DoorObstructedState(this, _fsm));

		//TODO Determine Starting state
		if (_currentDoorState == DoorStatus.Open) {
			_fsm.SetState<DoorOpenState>();
		} else if (_currentDoorState == DoorStatus.Closed) {
			_fsm.SetState<DoorClosedState>();
		};
	}

	private void Update() {
		_fsm.Step();
	}

	public void Interact() {
		_fsm.GetState().Interact();
		SendDoorUpdate();
	}

	public override void ChangeState(DoorStatus status)
	{
		base.ChangeState(status);
		if(status == DoorStatus.Open)
		{
			_fsm.SetState<DoorOpenState>();
		}
		else if(status == DoorStatus.Closed)
		{
			_fsm.SetState<DoorClosedState>();
		}
	}
}
