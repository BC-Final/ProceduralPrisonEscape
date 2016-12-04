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
	[SerializeField]
	private Transform _minimap;

	private Vector2 _minimapScale;
	private MinimapManager _minimapManager;
	private HackerPackageSender _networkManager;
	private Texture _minimapTexture;
	private MinimapPlayer _player;

	[SerializeField]
	private bool _showReceivedPackets;

	// Use this for initialization
	void Start () {
		_networkManager = FindObjectOfType<HackerPackageSender>();
		_minimapManager = GameObject.FindObjectOfType<MinimapManager>();
		_minimapManager.SetScale(2);
		_minimapManager.SetSender(_networkManager);
	}

	void Update () {
		try {
			if (HackerPackageSender.Host.Available != 0) {
				CustomCommands.AbstractPackage response = HackerPackageSender.Formatter.Deserialize(HackerPackageSender.Stream) as CustomCommands.AbstractPackage;
				ReadResponse(response);
			}
		} catch (Exception e) {
			Debug.LogError("Error" + e.ToString());
		}
	}
	//Message Management
	/// <summary>
	/// Incoming Packages are read here. They first enter as Abstrac Package.
	/// Then they get transmitted to another method that is made for their specific package.
	/// </summary>
	/// <param name="response"></param>
	private void ReadResponse (CustomCommands.AbstractPackage package) {
		//Debug.Log("Package Received : Abstract");

		string debugMessage = "ERROR!!! PACKAGE METHOD NOT FOUND OR IMPLEMENTED";
		//Update Methods
		if (package is CustomCommands.Update.DoorUpdate) { debugMessage = "Package Received : DoorUpdate"; ReadResponse(package as CustomCommands.Update.DoorUpdate); }
		if (package is CustomCommands.Update.FireWallUpdate) { debugMessage = "Package Received : FireWallUpdate"; ReadResponse(package as CustomCommands.Update.FireWallUpdate); }
		if (package is CustomCommands.Update.Items.ItemUpdate) { debugMessage = "Package Received : FireWallUpdate"; ReadResponse(package as CustomCommands.Update.Items.ItemUpdate); }
		if (package is CustomCommands.Update.MinimapUpdate) { debugMessage = "Package Received : MinimapUpdate"; ReadResponse(package as CustomCommands.Update.MinimapUpdate); }
		if (package is CustomCommands.Update.PlayerPositionUpdate) { debugMessage = "Package Received : PlayerPositionUpdate"; ReadResponse(package as CustomCommands.Update.PlayerPositionUpdate); }
		if (package is CustomCommands.Update.EnemyUpdate) { debugMessage = "Package Received : EnemyUpdate"; ReadResponse(package as CustomCommands.Update.EnemyUpdate); }

		//Creation Methods
		if (package is CustomCommands.Creation.DoorCreation) { debugMessage = "Package Received : DoorCreation"; ReadResponse(package as CustomCommands.Creation.DoorCreation); }
		if (package is CustomCommands.Creation.FireWallCreation) { debugMessage = "Package Received : FireWallCreation"; ReadResponse(package as CustomCommands.Creation.FireWallCreation); }
		if (package is CustomCommands.Creation.Items.KeyCardCreation) { debugMessage = "Package Received : FireWallCreation"; ReadResponse(package as CustomCommands.Creation.Items.KeyCardCreation); }
		if (package is CustomCommands.Creation.Items.HealthKitCreation) { debugMessage = "Package Received : FireWallCreation"; ReadResponse(package as CustomCommands.Creation.Items.HealthKitCreation); }
		if (package is CustomCommands.Creation.Items.AmmoPackCreation) { debugMessage = "Package Received : FireWallCreation"; ReadResponse(package as CustomCommands.Creation.Items.AmmoPackCreation); }
		if (package is CustomCommands.Creation.Shots.LaserShotCreation) { debugMessage = "Package Received : LaserShotCreation"; ReadResponse(package as CustomCommands.Creation.Shots.LaserShotCreation); }
		if (package is CustomCommands.Creation.NodeCreation) { debugMessage = "Package Received : NodeCreation"; ReadResponse(package as CustomCommands.Creation.NodeCreation); }
		if (package is CustomCommands.Creation.OnCreationEnd) { debugMessage = "Package Received : OnCreationEnd"; ReadResponse(package as CustomCommands.Creation.OnCreationEnd); }

		if (_showReceivedPackets) {
			Debug.Log(debugMessage);
		}
	}

	//Creation Methods
	private void ReadResponse (CustomCommands.Creation.DoorCreation package) {
		HackerDoor.CreateDoor(package);
	}

	private void ReadResponse (CustomCommands.Creation.FireWallCreation package) {
		HackerFireWall.CreateFireWall(package);
	}

	private void ReadResponse (CustomCommands.Creation.Shots.LaserShotCreation package) {
		MinimapManager.GetInstance().CreateShot(package);
	}

	//Item Creation Methods
	private void ReadResponse (CustomCommands.Creation.Items.KeyCardCreation package) {
		ItemDisplayIcon.CreateItem(package);
	}

	private void ReadResponse (CustomCommands.Creation.Items.HealthKitCreation package) {
		ItemDisplayIcon.CreateItem(package);
	}

	private void ReadResponse (CustomCommands.Creation.Items.AmmoPackCreation package) {
		ItemDisplayIcon.CreateItem(package);
	}


	private void ReadResponse (CustomCommands.Update.DoorUpdate package) {
		HackerDoor.UpdateDoor(package);
	}

	private void ReadResponse (CustomCommands.Update.FireWallUpdate package) {
		HackerFireWall.UpdateFireWall(package);
	}

	private void ReadResponse (CustomCommands.Update.Items.ItemUpdate package) {
		ItemDisplayIcon.UpdateItem(package);
	}

	private void ReadResponse (CustomCommands.Update.MinimapUpdate package) {
		Texture2D tex = new Texture2D(2, 2);
		tex.LoadImage(package.bytes);
		_minimap.GetComponent<Renderer>().material.mainTexture = tex;
	}

	private void ReadResponse (CustomCommands.Update.PlayerPositionUpdate package) {
		MinimapManager.GetInstance().UpdateMinimapPlayer(package);
	}

	private void ReadResponse (CustomCommands.Creation.NodeCreation pPackage) {
		NetworkWindow.Instance.AddNode(pPackage);
	}

	private void ReadResponse (CustomCommands.Creation.OnCreationEnd pPackage) {
		NetworkWindow.Instance.FinishedReceivingAll();
	}

	private void ReadResponse (CustomCommands.Update.EnemyUpdate package) {
		MinimapEnemy.UpdateEnemy(package);
	}
}



