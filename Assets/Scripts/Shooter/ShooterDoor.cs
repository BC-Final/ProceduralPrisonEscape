using UnityEngine;
using System.Collections;
using StateFramework;
using System.Collections.Generic;

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
		
	}

	[SerializeField]
	protected ShooterFireWall _firewall;

	[SerializeField]
	protected DoorState _currentDoorState;

	[SerializeField]
	public bool _requireKeyCard;

	private StateMachine<AbstractDoorState> _fsm = null;

	public Transform LeftDoor;
	public Transform RightDoor;

	private void Awake() {
		Initialize();
		_doors.Add(this);
	}

	private void OnDestroy() {
		_doors.Remove(this);
	}
	
	private void Start()
	{
		if (_firewall != null) {
			_firewall.AddDoor(this);
		}

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
		ShooterPackageSender.SendPackage(new CustomCommands.Update.DoorUpdate(Id, GetDoorState().ToString()));
	}

	public void ChangeState(DoorState state)
	{
		if (_firewall == null || _firewall.GetState()) {
			_currentDoorState = state;
		}

		if (state == DoorState.Open)
		{
			_fsm.SetState<DoorOpenState>();
		}
		else if(state == DoorState.Closed)
		{
			_fsm.SetState<DoorClosedState>();
		}
	}

	private void OnTriggerEnter(Collider pOther) {
		//TODO Activate only when door is not locked or protected
		if (pOther.GetComponent<Enemy_Drone>() != null) {
			ChangeState(DoorState.Open);
		}
	}

	private void OnTriggerExit(Collider pOther) {
		if (pOther.GetComponent<Enemy_Drone>() != null) {
			ChangeState(DoorState.Closed);
		}
	}

	public DoorState GetDoorState () {
		return _currentDoorState;
	}

	public void SetDoorState (DoorState pStatus) {
		_currentDoorState = pStatus;
	}

	public ShooterFireWall GetFireWall () {
		return _firewall;
	}

	public void SetRequireKeyCard () {
		_requireKeyCard = true;
	}



	public static void UpdateDoor (CustomCommands.Update.DoorUpdate update) {
		ShooterDoor door = GetDoorByID(update.ID);
		door.ChangeState(Helper.ParseEnum<DoorState>(update.state));
	}

	private static ShooterDoor GetDoorByID (int pId) {
		return _doors.Find(x => x.Id == pId);
	}
}
