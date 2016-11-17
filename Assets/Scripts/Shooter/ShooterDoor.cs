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
		if (_currentDoorState == DoorState.Open) {
			_fsm.SetState<DoorOpenState>();
		} else if (_currentDoorState == DoorState.Closed) {
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

	public override void ChangeState(DoorState status)
	{
		base.ChangeState(status);
		if(status == DoorState.Open)
		{
			_fsm.SetState<DoorOpenState>();
		}
		else if(status == DoorState.Closed)
		{
			_fsm.SetState<DoorClosedState>();
		}
	}

	private void OnTriggerEnter(Collider pOther) {
		//TODO Activate only when door is not locked or protected
		if (pOther.GetComponent<Enemy_Drone>() != null) {
			ChangeState(DoorStatus.Open);
		}
	}

	private void OnTriggerExit(Collider pOther) {
		if (pOther.GetComponent<Enemy_Drone>() != null) {
			ChangeState(DoorStatus.Closed);
		}
	}
}
