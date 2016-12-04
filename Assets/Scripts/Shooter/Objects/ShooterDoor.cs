using UnityEngine;
using System.Collections;
using StateFramework;
using System.Collections.Generic;
using System.Net.Sockets;

[SelectionBase]
public class ShooterDoor : MonoBehaviour, IInteractable, INetworked {
	private static List<ShooterDoor> _doors = new List<ShooterDoor>();
	public static List<ShooterDoor> GetDoorList () { return _doors; }

	[SerializeField]
	private int _id;
	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}

	public void Initialize (TcpClient pClient) {
		ShooterPackageSender.SendPackage(new CustomCommands.Creation.DoorCreation(Id, transform.position.x, transform.position.z, transform.rotation.eulerAngles.y, (int)_currentDoorState), pClient);
	}

	[SerializeField]
	protected DoorState _currentDoorState;

	[SerializeField]
	public bool _requireKeyCard;

	private StateMachine<AbstractDoorState> _fsm = null;

	public Transform LeftDoor;
	public Transform RightDoor;

	private void Awake () {
		_doors.Add(this);
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy () {
		_doors.Remove(this);
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	private void Start () {
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


	private void Update () {
		_fsm.Step();
	}

	public void Interact () {
		_fsm.GetState().Interact();
		ShooterPackageSender.SendPackage(new CustomCommands.Update.DoorUpdate(Id, (int)GetDoorState()));
	}

	public void ChangeState (DoorState state) {
		_currentDoorState = state;

		if (state == DoorState.Open) {
			_fsm.SetState<DoorOpenState>();
		} else if (state == DoorState.Closed) {
			_fsm.SetState<DoorClosedState>();
		}
	}

	private void OnTriggerEnter (Collider pOther) {
		//TODO Activate only when door is not locked or protected
		if (pOther.GetComponent<DroneEnemy>() != null) {
			ChangeState(DoorState.Open);
		}
	}

	private void OnTriggerExit (Collider pOther) {
		if (pOther.GetComponent<DroneEnemy>() != null) {
			ChangeState(DoorState.Closed);
		}
	}

	public DoorState GetDoorState () {
		return _currentDoorState;
	}

	public void SetDoorState (DoorState pStatus) {
		_currentDoorState = pStatus;
	}

	public void SetRequireKeyCard () {
		_requireKeyCard = true;
	}


	public static void UpdateDoor (CustomCommands.Update.DoorUpdate pUpdate) {
		ShooterDoor door = GetDoorByID(pUpdate.ID);
		door.ChangeState((DoorState)pUpdate.state);
	}


	private static ShooterDoor GetDoorByID (int pId) {
		return _doors.Find(x => x.Id == pId);
	}
}
