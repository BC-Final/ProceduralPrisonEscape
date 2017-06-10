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
	private void Start() {
		StartCoroutine(readNetwork());
	}

	/// <summary>
	/// Read incomming packages
	/// </summary>
	//private void Update() {
	//	if (ShooterPackageSender.Client != null) {
	//		try {
	//			if (ShooterPackageSender.Client.Available != 0) {
	//				//FIX Does this really only read one package per frame???
	//				NetworkPacket.AbstractPacket response = ShooterPackageSender.Formatter.Deserialize(ShooterPackageSender.Client.GetStream()) as NetworkPacket.AbstractPacket;
	//				readPacket(response);
	//			}
	//		} catch (SocketException e) {
	//			Debug.LogError("Error" + e.ToString());
	//		}
	//	}
	//}

	private IEnumerator readNetwork() {
		while (true) {
			if (ShooterPackageSender.Client != null) {
				try {
					if (ShooterPackageSender.Client.Available != 0) {
						NetworkPacket.AbstractPacket response = ShooterPackageSender.Formatter.Deserialize(ShooterPackageSender.Client.GetStream()) as NetworkPacket.AbstractPacket;
						readPacket(response);
					}
				} catch (SocketException e) {
					Debug.LogError("Error" + e.ToString());
				}
			}

			yield return null;
		}
	}

	/// <summary>
	/// Checks a package for its type and
	/// </summary>
	/// <param name="pPacket"></param>
	private void readPacket(NetworkPacket.AbstractPacket pPacket) {
		if (pPacket is NetworkPacket.Update.Door) { readPacket(pPacket as NetworkPacket.Update.Door); }
		if (pPacket is NetworkPacket.Update.SectorDoor) { readPacket(pPacket as NetworkPacket.Update.SectorDoor); }
		if (pPacket is NetworkPacket.Update.Pipe) { readPacket(pPacket as NetworkPacket.Update.Pipe); }
		if (pPacket is NetworkPacket.Update.Turret) { readPacket(pPacket as NetworkPacket.Update.Turret); }
		if (pPacket is NetworkPacket.Update.Camera) { readPacket(pPacket as NetworkPacket.Update.Camera); }
		if (pPacket is NetworkPacket.Update.CodeLockCode) { readPacket(pPacket as NetworkPacket.Update.CodeLockCode); }
		if (pPacket is NetworkPacket.Update.ButtonPush) { readPacket(pPacket as NetworkPacket.Update.ButtonPush); }
		if (pPacket is NetworkPacket.Update.Grenade) { readPacket(pPacket as NetworkPacket.Update.Grenade); }
		if (pPacket is NetworkPacket.Update.Fusebox) { readPacket(pPacket as NetworkPacket.Update.Fusebox); }

		if (pPacket is NetworkPacket.Update.SecurityStationHackerInteract) { readPacket(pPacket as NetworkPacket.Update.SecurityStationHackerInteract); }

		//if (package is CustomCommands.Update.DoorUpdate) { debugMessage = "Package Received : DoorUpdate"; ReadPackage(package as CustomCommands.Update.DoorUpdate); return; }
		//if (package is CustomCommands.Update.DisableCamera) { debugMessage = "Package Received : CameraState"; ReadPackage(package as CustomCommands.Update.DisableCamera); return; }
		//if (package is CustomCommands.Update.DisableTurret) { debugMessage = "Package Received : CameraState"; ReadPackage(package as CustomCommands.Update.DisableTurret); return; }
		//if (package is CustomCommands.Update.AlarmUpdate) { debugMessage = "Package Received : CameraState"; ReadPackage(package as CustomCommands.Update.AlarmUpdate); return; }
		if (pPacket is NetworkPacket.GameObjects.Lasergate.hUpdate) {(pPacket as NetworkPacket.GameObjects.Lasergate.hUpdate).Invoke(); }
	}

	private void readPacket(NetworkPacket.Update.Door pPacket) {
		ShooterDoor.ProcessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Update.Grenade pPacket) {
		ShooterGrenade.ProcessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Update.Fusebox pPacket) {
		Fusebox.ProcessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Update.SectorDoor pPacket) {
		ShooterSectorDoor.ProcessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Update.Turret pPacket) {
		Turret.ProcessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Update.Camera pPacket) {
		ShooterCamera.ProcessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Update.Pipe pPacket) {
		ShooterPipe.ProcessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Update.CodeLockCode pPacket) {
		CodeLock.ProcessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Update.ButtonPush pPacket) {
		ButtonTerminal.ProccessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Update.SecurityStationHackerInteract pPacket) {
		SecurityStation.ProcessPacket(pPacket);
	}








	//private void ReadPackage (CustomCommands.Update.DoorUpdate package) {
	//	//ShooterDoor.UpdateDoor(package);
	//}

	//private void ReadPackage (CustomCommands.Update.DisableCamera package) {
	//	//HACK This is very hacky, create list of all cameras
	//	new List<ShooterCamera>(FindObjectsOfType<ShooterCamera>()).Find(x => x.Id == package.Id).Disable();
	//}

	//private void ReadPackage (CustomCommands.Update.DisableTurret package) {
	//	//HACK This is very hacky, create list of all cameras
	//	new List<Turret>(FindObjectsOfType<Turret>()).Find(x => x.Id == package.Id).Disable();
	//}

	//private void ReadPackage (CustomCommands.Update.AlarmUpdate package) {
	//	if (package.state) {
	//		AlarmManager.Instance.ActivateAlarm();
	//	} else {
	//		AlarmManager.Instance.DeactivateAlarm();
	//	}
	//}
}