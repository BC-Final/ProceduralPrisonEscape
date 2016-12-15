using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

public class ShooterPackageReader : MonoBehaviour {
	[SerializeField]
	private bool _showReceivedPackets;

	/// <summary>
	/// Read incomming packages
	/// </summary>
	private void Update () {
		if (ShooterPackageSender.Client != null) {
			try {
				if (ShooterPackageSender.Client.Available != 0) {
					//FIX Does this really only read one package per frame???
					CustomCommands.AbstractPackage response = ShooterPackageSender.Formatter.Deserialize(ShooterPackageSender.Client.GetStream()) as CustomCommands.AbstractPackage;
					ReadPackage(response);
				}
			} catch (SocketException e) {
				Debug.LogError("Error" + e.ToString());
			}
		}
	}

	/// <summary>
	/// Checks a package for its type and
	/// </summary>
	/// <param name="package"></param>
	private void ReadPackage (CustomCommands.AbstractPackage package) {
		string debugMessage = "ERROR!!! PACKAGE METHOD NOT FOUND OR IMPLEMENTED";

		if (package is CustomCommands.Update.DoorUpdate) { debugMessage = "Package Received : DoorUpdate"; ReadPackage(package as CustomCommands.Update.DoorUpdate); return; }
		if (package is CustomCommands.DisconnectPackage) { debugMessage = "Package Received : DisconnectPackage"; ReadPackage(package as CustomCommands.DisconnectPackage); return; }

		if (_showReceivedPackets) {
			Debug.Log(debugMessage);
		}
	}

	private void ReadPackage (CustomCommands.Update.DoorUpdate package) {
		ShooterDoor.UpdateDoor(package);
	}

	private void ReadPackage (CustomCommands.DisconnectPackage package) {
		Debug.Log("Client disconnected.");
		//ShooterPackageSender.SilentlyDisconnect();
	}
}