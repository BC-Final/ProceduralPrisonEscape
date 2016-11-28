using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HackerFireWall {

	MinimapFirewall _minimapIcon;

	private static List<HackerFireWall> _firewalls = new List<HackerFireWall>();

	public int ID;
	private bool _destroyed;
	private List<HackerDoor> _doors;

	public void ChangeState(bool nDestroyed)
	{
		_destroyed = nDestroyed;
		_minimapIcon.ChangeState(nDestroyed);
		foreach(HackerDoor d in _doors)
		{
			d.UpdateDoorState();
		}
	}

	public bool GetPermission()
	{
		return _destroyed;
	}

	public void SetMinimapIcon(MinimapFirewall minimapFirewall)
	{
		_minimapIcon = minimapFirewall;
	}

	private void AddDoor(HackerDoor door)
	{
		if (_doors == null)
		{
			_doors = new List<HackerDoor>();
		}
		_doors.Add(door);
	}

	//STATIC METHODS

	public static void CreateFireWall(CustomCommands.Creation.FireWallCreation firewallCreation)
	{
		HackerFireWall firewall = new HackerFireWall();
		firewall.ID = firewallCreation.ID;
		firewall._destroyed = firewallCreation.state;

		//Create minimap icon
		Vector3 pos = new Vector3(firewallCreation.x, 0, firewallCreation.z);
		MinimapFirewall minimapFirewall = MinimapManager.GetInstance().CreateMinimapFirewall(pos, firewallCreation.ID);

		firewall.SetMinimapIcon(minimapFirewall);
		firewall._minimapIcon.ChangeState(firewallCreation.state);

		//Adding Firewall to list
		AddFirewall(firewall);

		//links each door to this firewall
		for (int i = 0; i < firewallCreation.doorIDs.Length; i++)
		{
			HackerDoor door = HackerDoor.GetDoorByID(firewallCreation.doorIDs[i]);
			door.SetFirewall(GetFireWallByID(firewallCreation.ID));
			firewall.AddDoor(door);
		}
	}
	public static void UpdateFireWall(CustomCommands.Update.FireWallUpdate package)
	{
		HackerFireWall firewall= GetFireWallByID(package.ID);
		firewall.ChangeState(package.destroyed);
	}
	public static HackerFireWall GetFireWallByID(int ID)
	{
		foreach (HackerFireWall f in _firewalls)
		{
			if (f.ID == ID)
			{
				return f;
			}
		}
		return null;
	}

	private static void AddFirewall(HackerFireWall firewall)
	{
		_firewalls.Add(firewall);
	}
}
