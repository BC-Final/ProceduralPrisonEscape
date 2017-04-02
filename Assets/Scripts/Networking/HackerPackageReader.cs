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

	bool loadingFinished = false;
	private static List<NetworkPacket.AbstractPacket> _latePackages = new List<NetworkPacket.AbstractPacket>();

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

	private void AddLatePackage (NetworkPacket.AbstractPacket packet) {
		_latePackages.Add(packet);
	}

	private void ReadLatePackages () {
		foreach (NetworkPacket.AbstractPacket packet in _latePackages) {
            readPacket(packet);
		}
		_latePackages.Clear();
	}

	private void OnCreationEnd () {
        Debug.Log("CreationEnd");
		loadingFinished = true;
		ReadLatePackages();
	}

	private void readPacket (NetworkPacket.AbstractPacket pPacket) {
        //Creation
        if (pPacket is NetworkPacket.Create.KeyCard) { readPacket(pPacket as NetworkPacket.Create.KeyCard); }
        if (pPacket is NetworkPacket.Create.LaserShot) { readPacket(pPacket as NetworkPacket.Create.LaserShot); }
        if (pPacket is NetworkPacket.Create.CodeLock) { readPacket(pPacket as NetworkPacket.Create.CodeLock); }
        //Update
        if (pPacket is NetworkPacket.Update.Camera) { readPacket(pPacket as NetworkPacket.Update.Camera); }
		if (pPacket is NetworkPacket.Update.Door) { readPacket(pPacket as NetworkPacket.Update.Door); }
		if (pPacket is NetworkPacket.Update.Drone) { readPacket(pPacket as NetworkPacket.Update.Drone); }
        if (pPacket is NetworkPacket.Update.Icon) { readPacket(pPacket as NetworkPacket.Update.Icon); }
        if (pPacket is NetworkPacket.Update.Minimap) { readPacket(pPacket as NetworkPacket.Update.Minimap); }
		if (pPacket is NetworkPacket.Update.Pipe) { readPacket(pPacket as NetworkPacket.Update.Pipe); }
		if (pPacket is NetworkPacket.Update.Player) { readPacket(pPacket as NetworkPacket.Update.Player); }
		if (pPacket is NetworkPacket.Update.Turret) { readPacket(pPacket as NetworkPacket.Update.Turret); }

        //Other
        if (pPacket is NetworkPacket.Message.CreationEnd) { readPacket(pPacket as NetworkPacket.Message.CreationEnd); }
        if (pPacket is NetworkPacket.Message.DisconnectRequest) { readPacket(pPacket as NetworkPacket.Message.DisconnectRequest); }
	}
	
    /// <summary>
	/// Incoming Packages are read here. They first enter as Abstract Package.
	/// Then they get transmitted to another method that is made for their specific package.
	/// </summary>
	/// <param name="response"></param>
	private void readPacket (NetworkPacket.Update.Camera pPacket) {
		CameraMapIcon.ProcessPacket(pPacket);
	}
	private void readPacket (NetworkPacket.Update.Door pPacket) {
		DoorMapIcon.ProcessPacket(pPacket);
	}
	private void readPacket (NetworkPacket.Update.Drone pPacket) {
		DroneMapIcon.ProcessPacket(pPacket);
	}
    private void readPacket(NetworkPacket.Update.Icon pPacket)
    {
        KeycardMapIcon.ProcessPacket(pPacket);
    }
    private void readPacket (NetworkPacket.Update.Minimap package) {
		Texture2D tex = new Texture2D(2, 2);
		tex.LoadImage(package.bytes);
		FindObjectOfType<Minimap>().GetComponent<Renderer>().material.mainTexture = tex;
	}
	private void readPacket (NetworkPacket.Update.Pipe pPacket) {
		GaspipeMapIcon.ProcessPacket(pPacket);
	}
	private void readPacket (NetworkPacket.Update.Player pPacket) {
		PlayerMapIcon.ProcessPacket(pPacket);
	}
	private void readPacket (NetworkPacket.Update.Turret pPacket) {
		TurretMapIcon.ProcessPacket(pPacket);
	}
	
    
    

    /// <summary>
    /// Creation Methods
    /// </summary>
    /// <param name="pPacket"></param>
    private void readPacket(NetworkPacket.Create.CodeLock pPacket)
    {
        if (loadingFinished)
        {
            
            DoorMapIcon.AddAddon(pPacket);
        }
        else
        {
            AddLatePackage(pPacket);
        }
    }
    private void readPacket(NetworkPacket.Create.KeyCard pPacket)
    {
        if (loadingFinished)
        {
            KeycardMapIcon.ProcessPacket(pPacket);
            DoorMapIcon.AddAddon(pPacket);
        }
        else
        {
            AddLatePackage(pPacket);
        }
    }
    private void readPacket(NetworkPacket.Create.LaserShot pPacket)
    {
        MinimapManager.Instance.CreateShot(pPacket);
    }

    /// other
    /// 
    private void readPacket(NetworkPacket.Message.CreationEnd pPackage)
    {
        OnCreationEnd();
        //NetworkWindow.Instance.FinishedReceivingAll();
    }
    private void readPacket(NetworkPacket.Message.DisconnectRequest pPacket)
    {
        HackerPackageSender.SilentlyDisconnect();
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



