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
	/// <summary>
	/// Read incomming packages
	/// </summary>
	private void Update () {
		foreach (TcpClient client in ShooterPackageSender.Clients) {
			try {
				if (client.Available != 0) {
					//FIX Does this really only read one package per frame???
					CustomCommands.AbstractPackage response = ShooterPackageSender.Formatter.Deserialize(client.GetStream()) as CustomCommands.AbstractPackage;
					ReadPackage(response);
				}
			} catch (Exception e) {
				Debug.Log("Error" + e.ToString());
			}
		}
	}

	/// <summary>
	/// Checks a package for its type and
	/// </summary>
	/// <param name="package"></param>
	private void ReadPackage (CustomCommands.AbstractPackage package) {
		if (package is CustomCommands.Update.DoorUpdate) { ReadPackage(package as CustomCommands.Update.DoorUpdate); return; }

		//If package method not found
		Debug.Log("ERROR!!! NOT SUITABLE METHOD FOR THIS PACKAGE FOUND");
	}

	private void ReadPackage (CustomCommands.Update.DoorUpdate package) {
		ShooterDoor.UpdateDoor(package);
	}
}