using UnityEngine;
using System.Collections;

public class HackerDoorManager : DoorManager {

	MinimapManager _minimapManager;
	NetworkViewManager _networkViewManager;

	// Use this for initialization
	void Start () {
		_minimapManager = GameObject.FindObjectOfType<MinimapManager>();
		_networkViewManager = GameObject.FindObjectOfType<NetworkViewManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateDoor(Vector3 pos, float rotationY, string state, int ID)
	{
		HackerDoor door = new HackerDoor();
		door.Id = ID;
		
		//Creating Minimap Door
		MinimapDoor minimapDoor = _minimapManager.CreateMinimapDoor(pos, rotationY, ID);
		minimapDoor.SetMainDoor(door);
		door.SetMinimapDoor(minimapDoor);

		//Creating Networkview Door

		//Adding Door
		AddDoor(door);
		door.ChangeState(Door.ParseEnum<Door.DoorStatus>(state));


	}


}
