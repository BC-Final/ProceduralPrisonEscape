using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamelogic.Extensions;

public class HackerDoor {

	#region Static Fields
	private static List<HackerDoor> _doors = new List<HackerDoor>();
	#endregion

	#region References
	private MinimapDoor _minimapDoor;
	private DoorNode _doorNode;
	#endregion

	#region Private Fields
	private ObservedValue<DoorState> _state;
	private int _id;
	#endregion



	#region Properties
	/// <summary>
	/// Gets the network id of this door
	/// </summary>
	public int Id {
		get { return _id; }
	}



	/// <summary>
	/// Gets the state of this door
	/// </summary>
	public ObservedValue<DoorState> State {
		get { return _state; }
	}



	/// <summary>
	/// Sets the reference to the door node
	/// </summary>
	public DoorNode DoorNode {
		set { _doorNode = value; }
	}
	#endregion



	/// <summary>
	/// Sets the state of the door and sends and update to the shooter.
	/// The state only gets changed if the corresponding network node is accessed
	/// </summary>
	/// <param name="pState"></param>
	public void SetState (DoorState pState) {
		if (_doorNode.Accessed) {
			_state.Value = pState;
			HackerPackageSender.GetInstance().SendDoorUpdate(this);
		}
	}



	/// <summary>
	/// Creates a door and its minimap icon.
	/// </summary>
	/// <param name="pPackage">The information about the door</param>
	public static void CreateDoor (CustomCommands.Creation.DoorCreation pPackage) {
		HackerDoor door = new HackerDoor();
		door._id = pPackage.ID;

		MinimapDoor minimapDoor = MinimapManager.GetInstance().CreateMinimapDoor(new Vector3(pPackage.x, 0, pPackage.z), pPackage.rotationY, pPackage.ID);

		door._state = new ObservedValue<DoorState>((DoorState)pPackage.state);

		minimapDoor.AccociatedDoor = door;
		door._minimapDoor = minimapDoor;

		_doors.Add(door);
	}



	/// <summary>
	/// Updates the door and its icons.
	/// </summary>
	/// <param name="package">The information about the door</param>
	public static void UpdateDoor (CustomCommands.Update.DoorUpdate package) {
		HackerDoor door = GetDoorByID(package.ID);
		door._state.Value = (DoorState)package.state;
	}



	/// <summary>
	/// Finds a door with given id.
	/// </summary>
	/// <param name="pId">The id of the searched door</param>
	/// <returns>The found door, otherwise null</returns>
	public static HackerDoor GetDoorByID (int pId) {
		return _doors.Find(x => x.Id == pId);
	}
}
