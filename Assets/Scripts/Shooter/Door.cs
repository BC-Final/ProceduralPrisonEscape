using UnityEngine;
using StateFramework;


public class Door : MonoBehaviour {
	private StateMachine<AbstractDoorState> _fsm = null;

	private void Start () {
		_fsm = new StateMachine<AbstractDoorState>();

		_fsm.AddState(new DoorOpenState(this, _fsm));
		_fsm.AddState(new DoorClosedState(this, _fsm));
		_fsm.AddState(new DoorLockedState(this, _fsm));
		_fsm.AddState(new DoorProtectedState(this, _fsm));
		_fsm.AddState(new DoorObstructedState(this, _fsm));

		//TODO Determine Starting state
	}
}
