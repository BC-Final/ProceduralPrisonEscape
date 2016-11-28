using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour {

	private static List<Door> _doors;
	private static int _doorIndex = 0;

	public enum DoorState
	{
		Open,
		Closed,
	};

	[SerializeField]
	protected FireWall _firewall;
	[SerializeField]
	protected DoorState _currentDoorState;
	public int doorID;

	[SerializeField]
	public bool _requireKeyCard;

	public virtual void Start()
	{
		if (_firewall != null)
		{
			_firewall.AddDoor(this);
		}
	}

	public virtual void ChangeState(Door.DoorState state)
	{
		if (_firewall == null || _firewall.GetState())
		{
			_currentDoorState = state;
		}
		else
		{
			Debug.Log("Access Denied");
		}
	}

	public DoorState GetDoorState()
	{
		return _currentDoorState;
	}

	public void SetDoorState(Door.DoorState pStatus) {
		_currentDoorState = pStatus;
	}

	public FireWall GetFireWall()
	{
		return _firewall;
	}

	public void SetFireWall(FireWall firewall)
	{
		_firewall = firewall;
	}

	/// <summary>
	/// Sends Door update to hacker
	/// </summary>
	public void SendDoorUpdate()
	{
		ShooterPackageSender.SendPackage(new CustomCommands.Update.DoorUpdate(doorID, GetDoorState().ToString()));
	}

	//Static methods

	public static void UpdateDoor(CustomCommands.Update.DoorUpdate update)
	{
		Door door = GetDoorByID(update.ID);
		door.ChangeState(Door.ParseEnum<Door.DoorState>(update.state));
	}

	public static List<Door> GetDoorList()
	{
		if(_doors == null)
		{
			_doors = GetAllDoorsInLevel();
		}
		return _doors;
	}

	public static T ParseEnum<T>(string value)
	{
		return (T)Door.DoorState.Parse(typeof(T), value, true);
	}

	private static Door GetDoorByID(int ID)
	{
		foreach (Door d in _doors)
		{
			if (d.doorID == ID)
			{
				return d;
			}
		}
		return null;
	}

	private static List<Door> GetAllDoorsInLevel()
	{
		List<Door> allDoors = new List<Door>();
		Door[] doorArray = FindObjectsOfType<Door>();
		for (int i = 0; i < doorArray.Length; i++)
		{
			doorArray[i].doorID = _doorIndex;
			_doorIndex++;
			allDoors.Add(doorArray[i]);
		}
		_doorIndex = 0;
		return allDoors;
	}

	public void SetRequireKeyCard() {
		_requireKeyCard = true;
	}
}
