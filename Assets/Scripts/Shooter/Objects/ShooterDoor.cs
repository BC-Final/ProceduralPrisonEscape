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

	public void Initialize () {
		ShooterPackageSender.SendPackage(new CustomCommands.Creation.DoorCreation(Id, transform.position.x, transform.position.z, transform.rotation.eulerAngles.y, (int)_fsm.GetState().AssociatedState));
	}

	[SerializeField]
	public bool _requireKeyCard;

	private StateMachine<AbstractDoorState> _fsm = null;

    public GameObject FrontTerminal;
    public GameObject RearTerminal;
    public Transform LeftDoor;
	public Transform RightDoor;

	private void Awake () {
		_doors.Add(this);
		ShooterPackageSender.RegisterNetworkObject(this);
        FrontTerminal.SetActive( false);
        RearTerminal.SetActive(false);
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

		_fsm.SetState<DoorClosedState>();
	}


	private void Update () {
		_fsm.Step();
	}

	public void Interact () {
		_fsm.GetState().Interact();
	}

	public void SendStateUpdate (DoorState state) {
		ShooterPackageSender.SendPackage(new CustomCommands.Update.DoorUpdate(Id, (int)state));
	}

	private void OnTriggerEnter (Collider pOther) {
		//TODO Activate only when door is not locked or protected
		if (pOther.GetComponent<DroneEnemy>() != null) {
			_fsm.SetState<DoorOpenState>();
		}
	}

	private void OnTriggerExit (Collider pOther) {
		if (pOther.GetComponent<DroneEnemy>() != null) {
			_fsm.SetState<DoorClosedState>();
		}
	}

	public void SetRequireKeyCard () {
		_requireKeyCard = true;
        FrontTerminal.SetActive(true);
        RearTerminal.SetActive(true);
	}

	public static void UpdateDoor (CustomCommands.Update.DoorUpdate pUpdate) {
		ShooterDoor door = GetDoorByID(pUpdate.ID);
		AbstractDoorState newState = door._fsm.States.Find(x => x.AssociatedState == (DoorState)pUpdate.state);

		if (newState.GetType() != door._fsm.GetState().GetType()) {
			door._fsm.SetState(newState.GetType());
		}
	}


	private static ShooterDoor GetDoorByID (int pId) {
		return _doors.Find(x => x.Id == pId);
	}
}
