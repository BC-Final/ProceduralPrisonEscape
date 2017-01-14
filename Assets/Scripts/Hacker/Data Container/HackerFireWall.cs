using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamelogic.Extensions;

public class HackerFirewall {
	#region Static Fields
	private static List<HackerFirewall> _firewalls = new List<HackerFirewall>();
	#endregion

	#region References
	#pragma warning disable 0414
	private MinimapFirewall _minimapIcon;
	private FirewallNode _firewallNode;
	#pragma warning restore 0414
	#endregion

	#region Private Fields
	private ObservedValue<bool> _destroyed;
	private int _id;
	#endregion



	#region Properties
	/// <summary>
	/// Gets the network id of this object
	/// </summary>
	public int Id {
		get { return _id; }
	}



	/// <summary>
	/// Gets the state of this object
	/// </summary>
	public ObservedValue<bool> Destroyed {
		get { return _destroyed; }
	}

	/// <summary>
	/// Sets the reference to the firewall node
	/// </summary>
	public FirewallNode FirwallNode {
		set { _firewallNode = value; }
	}
	#endregion



	/// <summary>
	/// Creates a firewall and its minimap icon.
	/// </summary>
	/// <param name="pPackage">The information of the firewall</param>
	public static void CreateFireWall (CustomCommands.Creation.FireWallCreation pPackage) {
		HackerFirewall firewall = new HackerFirewall();
		firewall._id = pPackage.ID;

		MinimapFirewall minimapFirewall = MinimapManager.Instance.CreateMinimapFirewall(new Vector3(pPackage.x, 0, pPackage.z), pPackage.ID);

		firewall._destroyed = new ObservedValue<bool>(pPackage.state);

		minimapFirewall.AssociatedFirewall = firewall;
		firewall._minimapIcon = minimapFirewall;

		_firewalls.Add(firewall);
	}



	/// <summary>
	/// Updates the information about a firewall
	/// </summary>
	/// <param name="pPackage">The information about the firewall</param>
	public static void UpdateFireWall (CustomCommands.Update.FireWallUpdate pPackage) {
		HackerFirewall firewall = GetFireWallByID(pPackage.ID);

		firewall._destroyed.Value = pPackage.destroyed;

		if (pPackage.destroyed) {
			FMODUnity.RuntimeManager.CreateInstance("event:/PE_hacker/PE_hacker_firewall_shutdown").start();
		} else {
			FMODUnity.RuntimeManager.CreateInstance("event:/PE_hacker/PE_hacker_firewall_startup").start();
		}
	}



	/// <summary>
	/// Finds a firewall with given id
	/// </summary>
	/// <param name="pId">The id of the searched firewall</param>
	/// <returns>The found firewall, otherwise null</returns>
	public static HackerFirewall GetFireWallByID (int pId) {
		return _firewalls.Find(x => x.Id == pId);
	}
}
