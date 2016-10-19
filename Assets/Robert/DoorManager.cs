using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorManager : MonoBehaviour {


	public List<HackerDoor> _doors;
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
	}
		
	public HackerDoor GetDoorByID(int ID)
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
}
