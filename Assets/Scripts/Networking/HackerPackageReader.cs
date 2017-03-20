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

public class HackerPackageReader : MonoBehaviour {
	private Vector2 _minimapScale;
	private MinimapManager _minimapManager;
	private HackerPackageSender _networkManager;
	private Texture _minimapTexture;

	//private MinimapPlayer _player;

	//bool loadingFinished = false;
	//private static List<CustomCommands.AbstractPackage> _latePackages = new List<CustomCommands.AbstractPackage>();

	// Use this for initialization
	void Start () {
		_networkManager = FindObjectOfType<HackerPackageSender>();
		_minimapManager = GameObject.FindObjectOfType<MinimapManager>();
		_minimapManager.SetScale(2);
	}

	void Update () {
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

	//private void AddLatePackage (CustomCommands.AbstractPackage package) {
	//	_latePackages.Add(package);
	//}

	//private void ReadLatePackages () {
	//	foreach (CustomCommands.AbstractPackage package in _latePackages) {
	//		ReadResponse(package);
	//	}
	//	_latePackages.Clear();
	//}

	//private void OnCreationEnd () {
	//	loadingFinished = true;
	//	ReadLatePackages();
	//}

	/// <summary>
	/// Incoming Packages are read here. They first enter as Abstrac Package.
	/// Then they get transmitted to another method that is made for their specific package.
	/// </summary>
	/// <param name="response"></param>
	private void readPacket (NetworkPacket.AbstractPacket pPacket) {
		if (pPacket is NetworkPacket.Update.Door) { readPacket(pPacket as NetworkPacket.Update.Door); }
		if (pPacket is NetworkPacket.Update.Player) { readPacket(pPacket as NetworkPacket.Update.Player); }
		if (pPacket is NetworkPacket.Update.Drone) { readPacket(pPacket as NetworkPacket.Update.Drone); }
		if (pPacket is NetworkPacket.Update.Turret) { readPacket(pPacket as NetworkPacket.Update.Turret); }
		if (pPacket is NetworkPacket.Update.Camera) { readPacket(pPacket as NetworkPacket.Update.Camera); }
		if (pPacket is NetworkPacket.Update.Pipe) { readPacket(pPacket as NetworkPacket.Update.Pipe); }
        if (pPacket is NetworkPacket.Update.Minimap) { readPacket(pPacket as NetworkPacket.Update.Minimap); }

        //if (pPacket is NetworkPacket.Update.Minimap) { readPacket(pPacket as NetworkPacket.Update.Minimap); }

        if (pPacket is NetworkPacket.Message.DisconnectRequest) { readPacket(pPacket as NetworkPacket.Message.DisconnectRequest); }

		//string debugMessage = "ERROR!!! PACKAGE METHOD NOT FOUND OR IMPLEMENTED";
		////Update Methods
		//if (package is CustomCommands.Update.DoorUpdate) { debugMessage = "Package Received : DoorUpdate"; ReadResponse(package as CustomCommands.Update.DoorUpdate); }
		//if (package is CustomCommands.Update.FireWallUpdate) { debugMessage = "Package Received : FireWallUpdate"; ReadResponse(package as CustomCommands.Update.FireWallUpdate); }
		//if (package is CustomCommands.Update.Items.ItemUpdate) { debugMessage = "Package Received : FireWallUpdate"; ReadResponse(package as CustomCommands.Update.Items.ItemUpdate); }
		
		//if (package is CustomCommands.Update.PlayerPositionUpdate) { debugMessage = "Package Received : PlayerPositionUpdate"; ReadResponse(package as CustomCommands.Update.PlayerPositionUpdate); }

		//if (package is CustomCommands.Update.DroneUpdate) { debugMessage = "Package Received : EnemyUpdate"; ReadResponse(package as CustomCommands.Update.DroneUpdate); }
		//if (package is CustomCommands.Update.CameraUpdate) { debugMessage = "Package Received : CameraUpdate"; ReadResponse(package as CustomCommands.Update.CameraUpdate); }
		//if (package is CustomCommands.Update.TurretUpdate) { debugMessage = "Package Received : TurretUpdate"; ReadResponse(package as CustomCommands.Update.TurretUpdate); }

		////Creation Methods
		//if (package is CustomCommands.Creation.DroneCreation) { debugMessage = "Package Received : EnemyUpdate"; ReadResponse(package as CustomCommands.Creation.DroneCreation); }
		//if (package is CustomCommands.Creation.CameraCreation) { debugMessage = "Package Received : EnemyUpdate"; ReadResponse(package as CustomCommands.Creation.CameraCreation); }
		//if (package is CustomCommands.Creation.TurretCreation) { debugMessage = "Package Received : EnemyUpdate"; ReadResponse(package as CustomCommands.Creation.TurretCreation); }

		//if (package is CustomCommands.Creation.DoorCreation) { debugMessage = "Package Received : DoorCreation"; ReadResponse(package as CustomCommands.Creation.DoorCreation); }
		//if (package is CustomCommands.Creation.FireWallCreation) { debugMessage = "Package Received : FireWallCreation"; ReadResponse(package as CustomCommands.Creation.FireWallCreation); }
		//if (package is CustomCommands.Creation.Items.KeyCardCreation) { debugMessage = "Package Received : FireWallCreation"; ReadResponse(package as CustomCommands.Creation.Items.KeyCardCreation); }
		//if (package is CustomCommands.Creation.Items.HealthKitCreation) { debugMessage = "Package Received : FireWallCreation"; ReadResponse(package as CustomCommands.Creation.Items.HealthKitCreation); }
		//if (package is CustomCommands.Creation.Items.AmmoPackCreation) { debugMessage = "Package Received : FireWallCreation"; ReadResponse(package as CustomCommands.Creation.Items.AmmoPackCreation); }
		//if (package is CustomCommands.Creation.Shots.LaserShotCreation) { debugMessage = "Package Received : LaserShotCreation"; ReadResponse(package as CustomCommands.Creation.Shots.LaserShotCreation); }
		//if (package is CustomCommands.Creation.OnCreationEnd) { debugMessage = "Package Received : OnCreationEnd"; ReadResponse(package as CustomCommands.Creation.OnCreationEnd); }

		//if (package is CustomCommands.ServerShutdownPackage) { debugMessage = "Package Received : ServerShutdownPackage"; ReadResponse(package as CustomCommands.ServerShutdownPackage); }
		//if (package is CustomCommands.RefuseConnectionPackage) { debugMessage = "Package Received : RefuseConnectionPackage"; ReadResponse(package as CustomCommands.RefuseConnectionPackage); }
	}

	private void readPacket (NetworkPacket.Update.Door pPacket) {
		DoorMapIcon.ProcessPacket(pPacket);
	}

	private void readPacket (NetworkPacket.Update.Player pPacket) {
		PlayerMapIcon.ProcessPacket(pPacket);
	}

	private void readPacket (NetworkPacket.Update.Drone pPacket) {
		DroneMapIcon.ProcessPacket(pPacket);
	}

	private void readPacket (NetworkPacket.Update.Turret pPacket) {
		TurretMapIcon.ProcessPacket(pPacket);
	}

	private void readPacket (NetworkPacket.Update.Camera pPacket) {
		CameraMapIcon.ProcessPacket(pPacket);
	}

	private void readPacket (NetworkPacket.Update.Pipe pPacket) {
		GaspipeMapIcon.ProcessPacket(pPacket);
	}

	private void readPacket (NetworkPacket.Update.Minimap package) {
		Texture2D tex = new Texture2D(2, 2);
		tex.LoadImage(package.bytes);
		FindObjectOfType<Minimap>().GetComponent<Renderer>().material.mainTexture = tex;
	}

	private void readPacket (NetworkPacket.Message.DisconnectRequest pPacket) {
		HackerPackageSender.SilentlyDisconnect();
	}

    private void ReadResponse (NetworkPacket.Create.LaserShot pPacket) {
    	MinimapManager.Instance.CreateShot(pPacket);
    }


    //Creation Methods
    //private void ReadResponse (CustomCommands.Creation.DoorCreation package) {
    //	HackerDoor.CreateDoor(package);
    //}

    //private void ReadResponse (CustomCommands.Creation.FireWallCreation package) {
    //	if (loadingFinished) {
    //		HackerFirewall.CreateFireWall(package);
    //	} else {
    //		AddLatePackage(package);
    //	}

    //}



    //Item Creation Methods
    //private void ReadResponse (CustomCommands.Creation.Items.KeyCardCreation package) {
    //	if (loadingFinished) {
    //		ItemDisplayIcon.CreateItem(package);
    //	} else {
    //		AddLatePackage(package);
    //	}
    //}

    //private void ReadResponse (CustomCommands.Creation.Items.HealthKitCreation package) {
    //	ItemDisplayIcon.CreateItem(package);
    //}

    //private void ReadResponse (CustomCommands.Creation.Items.AmmoPackCreation package) {
    //	ItemDisplayIcon.CreateItem(package);
    //}


    //private void ReadResponse (CustomCommands.Update.DoorUpdate package) {
    //	HackerDoor.UpdateDoor(package);
    //}

    //private void ReadResponse (CustomCommands.Update.FireWallUpdate package) {
    //	HackerFirewall.UpdateFireWall(package);
    //}

    //private void ReadResponse (CustomCommands.Update.Items.ItemUpdate package) {
    //	ItemDisplayIcon.UpdateItem(package);
    //}

    //private void ReadResponse (CustomCommands.Update.PlayerPositionUpdate package) {
    //	MinimapManager.Instance.UpdateMinimapPlayer(package);
    //}

    /*
	private void ReadResponse (CustomCommands.Creation.NodeCreation pPackage) {
		NetworkWindow.Instance.AddNode(pPackage);
	}
	*/

    //private void ReadResponse (CustomCommands.Creation.OnCreationEnd pPackage) {
    //	OnCreationEnd();
    //	//NetworkWindow.Instance.FinishedReceivingAll();
    //}



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



