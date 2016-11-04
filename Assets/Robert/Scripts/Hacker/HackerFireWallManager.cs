using UnityEngine;
using System.Collections;
using CustomCommands.Update;

public class HackerFireWallManager : FireWallManager {

	MinimapManager _minimapManager;
	HackerDoorManager _hackerDoorManager;

	void Start () {
		_minimapManager = GameObject.FindObjectOfType<MinimapManager>();
		_hackerDoorManager = GameObject.FindObjectOfType<HackerDoorManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateFireWall(CustomCommands.Creation.FireWallCreation firewallCreation)
	{
		Debug.Log("Creating Firewall");
		HackerFireWall firewall = new HackerFireWall();
		firewall.ID = firewallCreation.ID;

		Debug.Log(firewallCreation.doorIDs.ToString());

		foreach(int i in firewallCreation.doorIDs)
		{
			Door door = _hackerDoorManager.GetDoorByID(i);
			Debug.Log("Found Door : " + door.ToString());
			door.SetFireWall(firewall);
			Debug.Log("Firewall set : " + firewall.ToString());
			firewall.AddDoor(door);
			Debug.Log("Door added : " + door);
		}

		Vector3 pos = new Vector3(firewallCreation.x, 0, firewallCreation.z);
		MinimapFirewall minimapFirewall = _minimapManager.CreateMinimapFirewall(pos/_minimapManager.scale, firewallCreation.ID);
		minimapFirewall.ChangeState(firewallCreation.state);

		firewall.SetMinimapIcon(minimapFirewall);

		_fireWalls.Add(firewall);
	}
}
