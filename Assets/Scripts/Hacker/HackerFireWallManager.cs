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
		_fireWalls.Add(firewall);
		Debug.Log(firewallCreation.doorIDs[0] + "  " + firewallCreation.doorIDs[1]);

		for(int i = 0; i<firewallCreation.doorIDs.Length;i++)
		{
			Debug.Log("Firewall creation. doorIDs["+i+"] = "+firewallCreation.doorIDs[i]);
			Door door = _hackerDoorManager.GetDoorByID(firewallCreation.doorIDs[i]);
			door.SetFireWall(GetFireWallByID(firewallCreation.ID));
			firewall.AddDoor(door);
		}

		Vector3 pos = new Vector3(firewallCreation.x, 0, firewallCreation.z);
		MinimapFirewall minimapFirewall = _minimapManager.CreateMinimapFirewall(pos/_minimapManager.scale, firewallCreation.ID);
		minimapFirewall.ChangeState(firewallCreation.state);

		firewall.SetMinimapIcon(minimapFirewall);

		
	}
}
