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
using System.IO;

public class HackerPackageReader : MonoBehaviour {
	private static HackerPackageReader _instance;
	public static HackerPackageReader GetInstance()
	{
		if (!_instance)
		{
			_instance = GameObject.FindObjectOfType<HackerPackageReader>();
		}
		return _instance;
	}

	private Vector2 _minimapScale;
	private MinimapManager _minimapManager;
	private HackerPackageSender _networkManager;
	private Texture _minimapTexture;

	//Replace once hacker has a real HUD
	private HUDCardHolder _cardHolder;
	public bool LoggerOn;
	private float _bytesRead;

	//private MinimapPlayer _player;

	bool loadingFinished = false;
	private static List<NetworkPacket.AbstractPacket> _latePackages = new List<NetworkPacket.AbstractPacket>();

	// Use this for initialization
	void Start() {
		_networkManager = FindObjectOfType<HackerPackageSender>();
		_minimapManager = GameObject.FindObjectOfType<MinimapManager>();
		_minimapManager.SetScale(2);
		StartCoroutine(readNetwork());
	}

	//void Update() {
	//	//try {
	//	while (HackerPackageSender.Host.Available > 0) {
	//		NetworkPacket.AbstractPacket response = HackerPackageSender.Formatter.Deserialize(HackerPackageSender.Host.GetStream()) as NetworkPacket.AbstractPacket;
	//		readPacket(response);
	//	}
	//	//} catch (Exception e) {
	//	//Debug.LogError("Error" + e.ToString());
	//	//}
	//}

	private IEnumerator readNetwork() {
		while (true) {
			if (HackerPackageSender.Host != null) {
				try {
					NetworkStream stream = HackerPackageSender.Host.GetStream();
					//long byteCount = stream.Position;

					while (HackerPackageSender.Host.Available != 0) {
						

						NetworkPacket.AbstractPacket response = HackerPackageSender.Formatter.Deserialize(stream) as NetworkPacket.AbstractPacket;
						_bytesRead = ToByteArray(response).Length;
						Debug.Log("bytes received : " + _bytesRead);
						readPacket(response);
					}

					//Debug.Log("bytecount :" + byteCount);
				} catch (SocketException e) {
					Debug.LogError("Error" + e.ToString());
				}
			}

			yield return null;
		}
	}

	// Convert an object to a byte array
	private static byte[] ToByteArray(NetworkPacket.AbstractPacket obj)
	{
		BinaryFormatter bf = new BinaryFormatter();
		using (var ms = new MemoryStream())
		{
			bf.Serialize(ms, obj);
			return ms.ToArray();
		}
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

	public void OnCreationEnd() {
		Debug.Log("CreationEnd");
		loadingFinished = true;
		ReadLatePackages();
	}

	/// <summary>
	/// Incoming Packages are read here. They first enter as Abstract Package.
	/// Then they get transmitted to another method that is made for their specific package.
	/// </summary>
	/// <param name="response"></param>
	private void readPacket(NetworkPacket.AbstractPacket pPacket)
	{
		////Creation
		//if (pPacket is NetworkPacket.Create.KeyCard) { readPacket(pPacket as NetworkPacket.Create.KeyCard); }
		//if (pPacket is NetworkPacket.Create.LaserShot) { readPacket(pPacket as NetworkPacket.Create.LaserShot); }
		//if (pPacket is NetworkPacket.Create.DecodeAddon) { readPacket(pPacket as NetworkPacket.Create.DecodeAddon); }
		//if (pPacket is NetworkPacket.Create.CodeLockAddon) { readPacket(pPacket as NetworkPacket.Create.CodeLockAddon); }
		//if (pPacket is NetworkPacket.Create.DuoButtonAddon) { readPacket(pPacket as NetworkPacket.Create.DuoButtonAddon); }
		//if (pPacket is NetworkPacket.Create.Firewall) { readPacket(pPacket as NetworkPacket.Create.Firewall); }
		//if (pPacket is NetworkPacket.Create.PhaserAmmoIcon) { readPacket(pPacket as NetworkPacket.Create.PhaserAmmoIcon); }
		//if (pPacket is NetworkPacket.Create.MachineGunAmmoIcon) { readPacket(pPacket as NetworkPacket.Create.MachineGunAmmoIcon); }
		//if (pPacket is NetworkPacket.Create.ShotgunAmmoIcon) { readPacket(pPacket as NetworkPacket.Create.ShotgunAmmoIcon); }
		//if (pPacket is NetworkPacket.Create.HealthKitIcon) { readPacket(pPacket as NetworkPacket.Create.HealthKitIcon); }
		//if (pPacket is NetworkPacket.Create.PushButton) { readPacket(pPacket as NetworkPacket.Create.PushButton); }
		//if (pPacket is NetworkPacket.Create.Fusebox) { readPacket(pPacket as NetworkPacket.Create.Fusebox); }
		//if (pPacket is NetworkPacket.Create.SecurityStation) { readPacket(pPacket as NetworkPacket.Create.SecurityStation); }
		//
		////Update
		//if (pPacket is NetworkPacket.Update.ButtonFeedback) { readPacket(pPacket as NetworkPacket.Update.ButtonFeedback); }
		//if (pPacket is NetworkPacket.Update.DeactivateDoor) { readPacket(pPacket as NetworkPacket.Update.DeactivateDoor); }
		//if (pPacket is NetworkPacket.Update.DisableDoorOptions) { readPacket(pPacket as NetworkPacket.Update.DisableDoorOptions); }
		//if (pPacket is NetworkPacket.Update.Door) { readPacket(pPacket as NetworkPacket.Update.Door); }
		//if (pPacket is NetworkPacket.Update.EnableDoorOptions) { readPacket(pPacket as NetworkPacket.Update.EnableDoorOptions); }
		//if (pPacket is NetworkPacket.Update.Firewall) { readPacket(pPacket as NetworkPacket.Update.Firewall); }
		//if (pPacket is NetworkPacket.Update.Fusebox) { readPacket(pPacket as NetworkPacket.Update.Fusebox); }
		//if (pPacket is NetworkPacket.Update.Grenade) { readPacket(pPacket as NetworkPacket.Update.Grenade); }
		//if (pPacket is NetworkPacket.Update.Icon) { readPacket(pPacket as NetworkPacket.Update.Icon); }
		//if (pPacket is NetworkPacket.Update.KeyCardCollected) { readPacket(pPacket as NetworkPacket.Update.KeyCardCollected); }
		//if (pPacket is NetworkPacket.Update.Player) { readPacket(pPacket as NetworkPacket.Update.Player); }
		//if (pPacket is NetworkPacket.Update.SectorDoor) { readPacket(pPacket as NetworkPacket.Update.SectorDoor); }
		//if (pPacket is NetworkPacket.Update.SecurityStation) { readPacket(pPacket as NetworkPacket.Update.SecurityStation); }
		//
		//
		////State
		//if (pPacket is NetworkPacket.States.AlarmState) { readPacket(pPacket as NetworkPacket.States.AlarmState); }
		//
		////Other
		//if (pPacket is NetworkPacket.Messages.CreationEnd) { readPacket(pPacket as NetworkPacket.Messages.CreationEnd); }
		//if (pPacket is NetworkPacket.Messages.DisconnectRequest) { readPacket(pPacket as NetworkPacket.Messages.DisconnectRequest); }
		//
		//if(pPacket is NetworkPacket.GameObjects.Lasergate.Creation) { (pPacket as NetworkPacket.GameObjects.Lasergate.Creation).Invoke(); }
		//if (pPacket is NetworkPacket.GameObjects.Lasergate.sUpdate) { (pPacket as NetworkPacket.GameObjects.Lasergate.sUpdate).Invoke(); }
		if(pPacket.isLatePacket && !loadingFinished)
		{
			AddLatePackage(pPacket);
		}
		else
		{
			pPacket.Invoke();
		}
	}

	/// <summary>
	/// UPDATE METHODS
	/// </summary>
	//private void readPacket(NetworkPacket.Update.ButtonFeedback pPacket) {
	//	PushButtonMapIcon.ProcessPacket(pPacket);
	//}
	//
	//private void readPacket(NetworkPacket.Update.DeactivateDoor pPacket) {
	//	if (loadingFinished)
	//	{
	//		DoorMapIcon.AddAddon(pPacket);
	//	} else {
	//		AddLatePackage(pPacket);
	//	}
	//}
	//private void readPacket(NetworkPacket.Update.DisableDoorOptions pPacket)
	//{
	//	if (loadingFinished)
	//	{
	//		DoorMapIcon.ProcessPacket(pPacket);
	//	}
	//	else
	//	{
	//		AddLatePackage(pPacket);
	//	}
	//}
	//private void readPacket(NetworkPacket.Update.Door pPacket) {
	//	DoorMapIcon.ProcessPacket(pPacket);
	//}
	//
	//private void readPacket(NetworkPacket.Update.EnableDoorOptions pPacket)
	//{
	//	if (loadingFinished)
	//	{
	//		DoorMapIcon.ProcessPacket(pPacket);
	//	}
	//	else
	//	{
	//		AddLatePackage(pPacket);
	//	}
	//}
	//private void readPacket(NetworkPacket.Update.Firewall pPacket)
	//{
	//	Debug.Log("I got something");
	//	FirewallMapIcon.ProcessPacket(pPacket);
	//}
	//private void readPacket(NetworkPacket.Update.Grenade pPacket) {
	//	GrenadeMapIcon.ProcessPacket(pPacket);
	//}
	//private void readPacket(NetworkPacket.Update.Icon pPacket) {
	//	AbstractMapIcon.ProcessPacket(pPacket);
	//}
	//private void readPacket(NetworkPacket.Update.KeyCardCollected pPacket)
	//{
	//	if (_cardHolder) { _cardHolder.AddCard(new Color(pPacket.colorR, pPacket.colorG, pPacket.colorB, 1)); } else { _cardHolder = GameObject.FindObjectOfType<HUDCardHolder>(); _cardHolder.AddCard(new Color(pPacket.colorR, pPacket.colorG, pPacket.colorB, 1)); }
	//}
	//
	//private void readPacket(NetworkPacket.Update.Player pPacket) {
	//	PlayerMapIcon.ProcessPacket(pPacket);
	//}
	//private void readPacket(NetworkPacket.Update.SectorDoor pPacket) {
	//	SectorDoorMapIcon.ProcessPacket(pPacket);
	//}
	//private void readPacket(NetworkPacket.Update.SecurityStation pPacket)
	//{
	//	SecuritystationMapIcon.ProcessPacket(pPacket);
	//}
	//
	///// <summary>
	///// Creation Methods
	///// </summary>
	///// <param name="pPacket"></param>
	///// 
	//private void readPacket(NetworkPacket.Create.CodeLockAddon pPacket) {
	//	if (loadingFinished) {
	//
	//		DoorMapIcon.AddAddon(pPacket);
	//	} else {
	//		AddLatePackage(pPacket);
	//	}
	//}
	//private void readPacket(NetworkPacket.Create.DecodeAddon pPacket) {
	//	if (loadingFinished) {
	//
	//		DoorMapIcon.AddAddon(pPacket);
	//	} else {
	//		AddLatePackage(pPacket);
	//	}
	//}
    //private void readPacket(NetworkPacket.Create.DuoButtonAddon pPacket)
    //{
    //    if (loadingFinished)
    //    {
	//
    //        DoorMapIcon.AddAddon(pPacket);
    //    }
    //    else
    //    {
    //        AddLatePackage(pPacket);
    //    }
    //}
	//private void readPacket(NetworkPacket.Create.Firewall pPacket)
	//{
	//	FirewallMapIcon.ProcessPacket(pPacket);
	//}
	//private void readPacket(NetworkPacket.Create.HealthKitIcon pPacket) {
	//	PickUpMapIcon.ProcessPacket(pPacket);
	//}
	//private void readPacket(NetworkPacket.Create.KeyCard pPacket) {
	//	if (loadingFinished) {
	//		KeycardMapIcon.ProcessPacket(pPacket);
	//		DoorMapIcon.AddAddon(pPacket);
	//	} else {
	//		AddLatePackage(pPacket);
	//	}
	//}
	//private void readPacket(NetworkPacket.Create.LaserShot pPacket) {
	//	MinimapManager.Instance.CreateShot(pPacket);
	//}
	//private void readPacket(NetworkPacket.Create.MachineGunAmmoIcon pPacket) {
	//	PickUpMapIcon.ProcessPacket(pPacket);
	//}
	//private void readPacket(NetworkPacket.Create.PushButton pPacket) {
	//	PushButtonMapIcon.ProcessPacket(pPacket);
	//}
    //private void readPacket(NetworkPacket.Create.PhaserAmmoIcon pPacket) {
	//	PickUpMapIcon.ProcessPacket(pPacket);
	//}
	//private void readPacket(NetworkPacket.Create.SecurityStation pPacket) {
	//	SecuritystationMapIcon.ProcessPacket(pPacket);
	//}
	//private void readPacket(NetworkPacket.Create.ShotgunAmmoIcon pPacket) {
	//	PickUpMapIcon.ProcessPacket(pPacket);
	//}
	//
	///// other
	///// 
	//private void readPacket(NetworkPacket.Messages.CreationEnd pPackage) {
	//	OnCreationEnd();
	//	//NetworkWindow.Instance.FinishedReceivingAll();
	//}
	//private void readPacket(NetworkPacket.Messages.DisconnectRequest pPacket) {
	//	HackerPackageSender.SilentlyDisconnect();
	//}
	//
	//private void readPacket(NetworkPacket.States.AlarmState pPacket) {
	//	HackerAlarmManager.Instance.ProcessPacket(pPacket);
	//}
	//private void readPacket(NetworkPacket.Messages.MoveCameraTowardsLocation pPacket)
	//{
	//	MinimapManager.Instance.ProcessPacket(pPacket);
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



