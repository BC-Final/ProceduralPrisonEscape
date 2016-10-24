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

	private List<HackerDoor> InitGetAllDoorsInLevel()
	{
		List<HackerDoor> allDoors = new List<HackerDoor>();
		HackerDoor[] doorArray = FindObjectsOfType<HackerDoor>();
		for(int i = 0; i < doorArray.Length; i++)
		{
			doorArray[i].Id = doorIndex;
			doorIndex++;
			allDoors.Add(doorArray[i]);
			doorArray[i].SetManager(this);
		}
		return allDoors;
	}

	public List<HackerDoor> GetDoorList()
	{
		return _doors;
	}

	public void AddDoor(HackerDoor nDoor)
	{
		_doors.Add(nDoor);
	}

	public void UpdateDoor(CustomCommands.DoorUpdate update)
	{
		HackerDoor door = GetDoorByID(update.ID);
		door.transform.position = new Vector3(update.x, door.transform.position.y, update.z);
		door.ChangeState(Door.ParseEnum<Door.DoorStatus>(update.state));
	}
		
<<<<<<< HEAD:Assets/Robert/DoorManager.cs
	public HackerDoor GetDoorByID(int ID)
=======
	public void UpdateDoorState(CustomCommands.DoorChangeState update)
	{
		Door door = GetDoorByID(update.ID);
		door.ChangeState(Door.ParseEnum<Door.DoorStatus>(update.state));
	}

	public void SendDoorStateUpdate(Door door)
	{
		_sender.SendDoorUpdate(door);
	}

	public Door GetDoorByID(int ID)
>>>>>>> robert:Assets/Robert/Scripts/GameObjects/General/DoorManager.cs
	{
		foreach(HackerDoor d in _doors)
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
		foreach(HackerDoor d in _doors)
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
