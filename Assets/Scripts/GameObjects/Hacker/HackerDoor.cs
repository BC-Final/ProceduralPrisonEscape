using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HackerDoor {

	private static List<HackerDoor> _doors = new List<HackerDoor>();

	private MinimapDoor _minimapDoor;
	private HackerFireWall _firewall;
	private Door.DoorState _currentDoorState;
	private int _id;

	public void ChangeState(Door.DoorState state)
	{
		if (GetFirewall() == null || _firewall.GetPermission())
		{
			_currentDoorState = state;
			_minimapDoor.ChangeState(state);
			HackerPackageSender.GetInstance().SendDoorUpdate(this);
		}
		else
		{
			//Door Access denied
		}
	}
	/// <summary>
	/// SetState is used when an update come in. ChangeState will send an update to the shooter.
	/// </summary>
	/// <param name="state"></param>
	public void SetState(Door.DoorState state)
	{
		_currentDoorState = state;
		_minimapDoor.ChangeState(state);
	}
	public void UpdateDoorState()
	{
		SetState(_currentDoorState);
	}

	//Setter
	public void SetMinimapDoor(MinimapDoor door)
	{
		_minimapDoor = door;
	}

	public void SetFirewall(HackerFireWall firewall)
	{
		_firewall = firewall;
		UpdateDoorState();
	}

	//Getter
	public HackerFireWall GetFirewall()
	{
		if(_firewall == null)
		{
			return null;
		}
		else
		{
			return _firewall;
		}
	}
	public int GetID()
	{
		return _id;
	}
	public Door.DoorState GetDoorState()
	{
		return _currentDoorState;
	}

	//STATIC METHODS

	public static void CreateDoor(CustomCommands.Creation.DoorCreation package)
	{
		//Check if door already exists
		if(GetDoorByID(package.ID) != null)
		{
			UpdateDoor(package);
			return;
		}

		//else create door
		HackerDoor door = new HackerDoor();
		door._id = package.ID;
		
		//Creating Minimap Door
		MinimapDoor minimapDoor = MinimapManager.GetInstance().CreateMinimapDoor(new Vector3(package.x,0, package.z), package.rotationY, package.ID);

		//Linking door and minimap icon
		minimapDoor.SetMainDoor(door);
		door.SetMinimapDoor(minimapDoor);

	
		//Adding door to door list
		AddDoor(door);
	
		//Syncing door state
		door.ChangeState(Door.ParseEnum<Door.DoorState>(package.state));
	}
	public static void UpdateDoor(CustomCommands.Update.DoorUpdate package)
	{
		HackerDoor door = GetDoorByID(package.ID);
		door.SetState(Door.ParseEnum<Door.DoorState>(package.state));
	}
	public static HackerDoor GetDoorByID(int ID)
	{
		foreach (HackerDoor d in _doors)
		{
			//Debug.Log("Searching for door" + d._id);
			if (d._id == ID)
			{
				//Debug.Log("Door found : " + d._id + " = " + ID);
				//Debug.Log("returning : " + d);
				return d;
			}
			//Debug.Log("Door NOT found : " + d._id + " != " + ID);
		}
		return null;
	}

	/// <summary>
	/// Should only be called in case or duplicate door creation
	/// </summary>
	/// <param name="package"></param>
	private static void UpdateDoor(CustomCommands.Creation.DoorCreation package)
	{
		HackerDoor door = GetDoorByID(package.ID);
		door.ChangeState(Door.ParseEnum<Door.DoorState>(package.state));
	}
	private static void AddDoor(HackerDoor door)
	{
		_doors.Add(door);
	}
}
