using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorManager : MonoBehaviour {

	private TCPMBTesterServer _sender;
	public List<Door> _doors;
	public int doorIndex = 0;
	// Use this for initialization
	void Start () {
		_doors = InitGetAllDoorsInLevel();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private List<Door> InitGetAllDoorsInLevel()
	{
		List<Door> allDoors = new List<Door>();
		Door[] doorArray = FindObjectsOfType<Door>();
		for(int i = 0; i < doorArray.Length; i++)
		{
			doorArray[i].Id = doorIndex;
			doorIndex++;
			allDoors.Add(doorArray[i]);
			doorArray[i].SetManager(this);
		}
		return allDoors;
	}

	public List<Door> GetDoorList()
	{
		return _doors;
	}

	public void AddDoor(Door nDoor)
	{
		_doors.Add(nDoor);
	}

	public void UpdateDoor(CustomCommands.Creation.DoorCreation update)
	{
		Door door = GetDoorByID(update.ID);
		door.transform.position = new Vector3(update.x, door.transform.position.y, update.z);
		door.ChangeState(Door.ParseEnum<Door.DoorStatus>(update.state));
	}
		
	public void UpdateDoorState(CustomCommands.Update.DoorUpdate update)
	{
		Door door = GetDoorByID(update.ID);
		door.ChangeState(Door.ParseEnum<Door.DoorStatus>(update.state));
	}

	public void SendDoorStateUpdate(Door door)
	{
		_sender.SendDoorUpdate(door);
	}

	public Door GetDoorByID(int ID)
	{
		foreach(Door d in _doors)
		{
			if(d.Id == ID)
			{
				return d;
			}
		}
		return null;
	}

	public bool DoorAlreadyExists(int ID)
	{
		if (_doors.Count == 0)
		{
			Debug.Log("DoorAlreadyExists : false");
			return false;
		}
		foreach(Door d in _doors)
		{
			if(d.Id == ID)
			{
				Debug.Log("DoorAlreadyExists : true");
				return true;
			}
		}
		Debug.Log("DoorAlreadyExists : false");
		return false;
	}

	public void SetSender(TCPMBTesterServer sender)
	{
		_sender = sender;
	}
}
