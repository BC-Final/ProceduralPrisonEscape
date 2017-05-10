﻿using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

public class HackerPackageReader : MonoBehaviour {
	private Vector2 _minimapScale;
	private MinimapManager _minimapManager;
	private HackerPackageSender _networkManager;
	private Texture _minimapTexture;

	//private MinimapPlayer _player;

	bool loadingFinished = false;
	private static List<NetworkPacket.AbstractPacket> _latePackages = new List<NetworkPacket.AbstractPacket>();

	// Use this for initialization
	void Start() {
		_networkManager = FindObjectOfType<HackerPackageSender>();
		_minimapManager = GameObject.FindObjectOfType<MinimapManager>();
		_minimapManager.SetScale(2);
	}

	void Update() {
		//try {
		while (HackerPackageSender.Host.Available > 0) {
			NetworkPacket.AbstractPacket response = HackerPackageSender.Formatter.Deserialize(HackerPackageSender.Host.GetStream()) as NetworkPacket.AbstractPacket;
			readPacket(response);
		}
		//} catch (Exception e) {
		//Debug.LogError("Error" + e.ToString());
		//}
	}

	///Late Package handling
	///packages that need to be read after door and other important packages, will be stored here and are being read after all packaged have been received

	private void AddLatePackage(NetworkPacket.AbstractPacket packet) {
		_latePackages.Add(packet);
	}

	private void ReadLatePackages() {
		foreach (NetworkPacket.AbstractPacket packet in _latePackages) {
			readPacket(packet);
		}
		_latePackages.Clear();
	}

	private void OnCreationEnd() {
		Debug.Log("CreationEnd");
		loadingFinished = true;
		ReadLatePackages();
	}

	private void readPacket(NetworkPacket.AbstractPacket pPacket) {
		//Creation
		if (pPacket is NetworkPacket.Create.KeyCard) { readPacket(pPacket as NetworkPacket.Create.KeyCard); }
		if (pPacket is NetworkPacket.Create.LaserShot) { readPacket(pPacket as NetworkPacket.Create.LaserShot); }
		if (pPacket is NetworkPacket.Create.DecodeAddon) { readPacket(pPacket as NetworkPacket.Create.DecodeAddon); }
		if (pPacket is NetworkPacket.Create.CodeLockAddon) { readPacket(pPacket as NetworkPacket.Create.CodeLockAddon); }
		if (pPacket is NetworkPacket.Create.PhaserAmmoIcon) { readPacket(pPacket as NetworkPacket.Create.PhaserAmmoIcon); }
		if (pPacket is NetworkPacket.Create.MachineGunAmmoIcon) { readPacket(pPacket as NetworkPacket.Create.MachineGunAmmoIcon); }
		if (pPacket is NetworkPacket.Create.ShotgunAmmoIcon) { readPacket(pPacket as NetworkPacket.Create.ShotgunAmmoIcon); }
		if (pPacket is NetworkPacket.Create.HealthKitIcon) { readPacket(pPacket as NetworkPacket.Create.HealthKitIcon); }
		if (pPacket is NetworkPacket.Create.PushButton) { readPacket(pPacket as NetworkPacket.Create.PushButton); }


		if (pPacket is NetworkPacket.Create.SecurityStation) { readPacket(pPacket as NetworkPacket.Create.SecurityStation); }

		//Update
		if (pPacket is NetworkPacket.Update.ButtonFeedback) { readPacket(pPacket as NetworkPacket.Update.ButtonFeedback); }
		if (pPacket is NetworkPacket.Update.Camera) { readPacket(pPacket as NetworkPacket.Update.Camera); }
		if (pPacket is NetworkPacket.Update.DisableDoor) { readPacket(pPacket as NetworkPacket.Update.DisableDoor); }
		if (pPacket is NetworkPacket.Update.Door) { readPacket(pPacket as NetworkPacket.Update.Door); }
		if (pPacket is NetworkPacket.Update.Drone) { readPacket(pPacket as NetworkPacket.Update.Drone); }
		if (pPacket is NetworkPacket.Update.Icon) { readPacket(pPacket as NetworkPacket.Update.Icon); }
		if (pPacket is NetworkPacket.Update.Minimap) { readPacket(pPacket as NetworkPacket.Update.Minimap); }
		if (pPacket is NetworkPacket.Update.SectorDoor) { readPacket(pPacket as NetworkPacket.Update.SectorDoor); }

		if (pPacket is NetworkPacket.Update.Pipe) { readPacket(pPacket as NetworkPacket.Update.Pipe); }
		if (pPacket is NetworkPacket.Update.Player) { readPacket(pPacket as NetworkPacket.Update.Player); }
		if (pPacket is NetworkPacket.Update.Turret) { readPacket(pPacket as NetworkPacket.Update.Turret); }

		if (pPacket is NetworkPacket.Update.Module) { readPacket(pPacket as NetworkPacket.Update.Module); }
		if (pPacket is NetworkPacket.Update.Objective) { readPacket(pPacket as NetworkPacket.Update.Objective); }

		if (pPacket is NetworkPacket.Update.Grenade) { readPacket(pPacket as NetworkPacket.Update.Grenade); }

		//State
		if (pPacket is NetworkPacket.States.AlarmState) { readPacket(pPacket as NetworkPacket.States.AlarmState); }

		//Other
		if (pPacket is NetworkPacket.Messages.CreationEnd) { readPacket(pPacket as NetworkPacket.Messages.CreationEnd); }
		if (pPacket is NetworkPacket.Messages.DisconnectRequest) { readPacket(pPacket as NetworkPacket.Messages.DisconnectRequest); }
	}

	/// <summary>
	/// Incoming Packages are read here. They first enter as Abstract Package.
	/// Then they get transmitted to another method that is made for their specific package.
	/// </summary>
	/// <param name="response"></param>
	private void readPacket(NetworkPacket.Update.Camera pPacket) {
		CameraMapIcon.ProcessPacket(pPacket);
	}
	private void readPacket(NetworkPacket.Update.Door pPacket) {
		DoorMapIcon.ProcessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Update.SectorDoor pPacket) {
		SectorDoorMapIcon.ProcessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Update.Grenade pPacket) {
		GrenadeMapIcon.ProcessPacket(pPacket);
	}




	private void readPacket(NetworkPacket.Update.ButtonFeedback pPacket) {
		PushButtonMapIcon.ProcessPacket(pPacket);
	}
	private void readPacket(NetworkPacket.Update.Drone pPacket) {
		DroneMapIcon.ProcessPacket(pPacket);
	}
	private void readPacket(NetworkPacket.Update.Icon pPacket) {
		AbstractMapIcon.ProcessPacket(pPacket);
	}
	private void readPacket(NetworkPacket.Update.Minimap package) {
		Texture2D tex = new Texture2D(2, 2);
		tex.LoadImage(package.bytes);
		FindObjectOfType<Minimap>().GetComponent<Renderer>().material.mainTexture = tex;
	}
	private void readPacket(NetworkPacket.Update.Pipe pPacket) {
		GaspipeMapIcon.ProcessPacket(pPacket);
	}
	private void readPacket(NetworkPacket.Update.Player pPacket) {
		PlayerMapIcon.ProcessPacket(pPacket);
	}
	private void readPacket(NetworkPacket.Update.Turret pPacket) {
		TurretMapIcon.ProcessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Update.Module pPacket) {
		ModuleMapIcon.ProcessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Update.Objective pPacket) {
		ObjectiveMapIcon.ProcessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Create.SecurityStation pPacket) {
		SecuritystationMapIcon.ProcessPacket(pPacket);
	}



	/// <summary>
	/// Creation Methods
	/// </summary>
	/// <param name="pPacket"></param>
	/// 
	private void readPacket(NetworkPacket.Create.PushButton pPacket) {
		Debug.Log("Packet read");
		PushButtonMapIcon.ProcessPacket(pPacket);
	}

	private void readPacket(NetworkPacket.Create.CodeLockAddon pPacket) {
		if (loadingFinished) {

			DoorMapIcon.AddAddon(pPacket);
		} else {
			AddLatePackage(pPacket);
		}
	}
	private void readPacket(NetworkPacket.Create.PhaserAmmoIcon pPacket) {
		PickUpMapIcon.ProcessPacket(pPacket);
	}
	private void readPacket(NetworkPacket.Create.MachineGunAmmoIcon pPacket) {
		PickUpMapIcon.ProcessPacket(pPacket);
	}
	private void readPacket(NetworkPacket.Create.ShotgunAmmoIcon pPacket) {
		PickUpMapIcon.ProcessPacket(pPacket);
	}
	private void readPacket(NetworkPacket.Create.HealthKitIcon pPacket) {
		PickUpMapIcon.ProcessPacket(pPacket);
	}
	private void readPacket(NetworkPacket.Create.DecodeAddon pPacket) {
		if (loadingFinished) {

			DoorMapIcon.AddAddon(pPacket);
		} else {
			AddLatePackage(pPacket);
		}
	}
	private void readPacket(NetworkPacket.Update.DisableDoor pPacket) {
		if (loadingFinished) {

			DoorMapIcon.AddAddon(pPacket);
		} else {
			AddLatePackage(pPacket);
		}
	}
	private void readPacket(NetworkPacket.Create.KeyCard pPacket) {
		if (loadingFinished) {
			KeycardMapIcon.ProcessPacket(pPacket);
			DoorMapIcon.AddAddon(pPacket);
		} else {
			AddLatePackage(pPacket);
		}
	}
	private void readPacket(NetworkPacket.Create.LaserShot pPacket) {
		MinimapManager.Instance.CreateShot(pPacket);
	}

	/// other
	/// 
	private void readPacket(NetworkPacket.Messages.CreationEnd pPackage) {
		OnCreationEnd();
		//NetworkWindow.Instance.FinishedReceivingAll();
	}
	private void readPacket(NetworkPacket.Messages.DisconnectRequest pPacket) {
		HackerPackageSender.SilentlyDisconnect();
	}

	private void readPacket(NetworkPacket.States.AlarmState pPacket) {
		HackerAlarmManager.Instance.ProcessPacket(pPacket);
	}


	//TODO Replace
	//private void ReadResponse (CustomCommands.Update.DroneUpdate package) {
	//	HackerDrone.UpdateDrone(package);
	//}

	//private void ReadResponse (CustomCommands.Update.CameraUpdate package) {
	//	HackerCamera.UpdateCamera(package.id, package.rotation, package.seesPlayer);
	//}

	//private void ReadResponse (CustomCommands.Update.TurretUpdate package) {
	//	HackerTurret.UpdateTurret(package);
	//}


	//TODO REPLACE
	//private void ReadResponse (CustomCommands.Creation.DroneCreation package) {
	//	HackerDrone.CreateDrone(package);
	//}

	//private void ReadResponse (CustomCommands.Creation.CameraCreation package) {
	//	HackerCamera.CreateCamera(package);
	//}

	//private void ReadResponse (CustomCommands.Creation.TurretCreation package) {
	//	HackerTurret.CreateTurret(package);
	//}
}



