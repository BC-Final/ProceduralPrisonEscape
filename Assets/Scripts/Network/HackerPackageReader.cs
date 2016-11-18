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
	private NetworkViewManager _networkViewManager;
	private HackerPackageSender _networkManager;
	[SerializeField]
	private Transform _minimap;
	private Texture _minimapTexture;
	private TcpClient _client;
	private NetworkStream _stream;
	private BinaryFormatter _formatter;
	private MinimapPlayer _player;

	// Use this for initialization
	void Start () {
		_networkManager = FindObjectOfType<HackerPackageSender>();
		_client = _networkManager.GetClient();
		_stream = _networkManager.GetStream();
		_formatter = _networkManager.GetFormatter();
		_minimapManager = GameObject.FindObjectOfType<MinimapManager>();
		_minimapManager.SetScale(4);
		_minimapManager.SetSender(_networkManager);
		
	}
	
	void Update () {
		try
		{
			if (_client.Available != 0)
			{
				CustomCommands.AbstractPackage response = _formatter.Deserialize(_stream) as CustomCommands.AbstractPackage;

				ReadResponse(response);
			}
		}
		catch (Exception e)
		{
			Debug.Log("Error" + e.ToString());
		}
	}
	//Message Management
	/// <summary>
	/// Incoming Packages are read here. They first enter as Abstrac Package.
	/// Then they get transmitted to another method that is made for their specific package.
	/// </summary>
	/// <param name="response"></param>

	//Abstract Method
	private void ReadResponse(CustomCommands.AbstractPackage package)
	{
		Debug.Log("Package Received : Abstract");
		//Creation Methods
		if (package is CustomCommands.Creation.DoorCreation) { Debug.Log("Package Received : DoorCreation"); ReadResponse(package as CustomCommands.Creation.DoorCreation); return; }
        if (package is CustomCommands.Creation.FireWallCreation) { Debug.Log("Package Received : FireWallCreation"); ReadResponse(package as CustomCommands.Creation.FireWallCreation); return; }
		if (package is CustomCommands.Creation.KeyCardCreation) { Debug.Log("Package Received : FireWallCreation"); ReadResponse(package as CustomCommands.Creation.KeyCardCreation); return; }

		//Update Methods
		if (package is CustomCommands.Update.DoorUpdate) { Debug.Log("Package Received : DoorUpdate"); ReadResponse(package as CustomCommands.Update.DoorUpdate); return; }
		if (package is CustomCommands.Update.FireWallUpdate) { Debug.Log("Package Received : FireWallUpdate"); ReadResponse(package as CustomCommands.Update.FireWallUpdate); return; }
		if (package is CustomCommands.Update.KeyCardUpdate) { Debug.Log("Package Received : FireWallUpdate"); ReadResponse(package as CustomCommands.Update.KeyCardUpdate); return; }
		if (package is CustomCommands.Update.MinimapUpdate) { Debug.Log("Package Received : MinimapUpdate"); ReadResponse(package as CustomCommands.Update.MinimapUpdate); return; }
		if (package is CustomCommands.Update.PlayerPositionUpdate) { Debug.Log("Package Received : PlayerPositionUpdate"); ReadResponse(package as CustomCommands.Update.PlayerPositionUpdate); return; }

		//Method not found or not implemented
		Debug.Log("ERROR!!! PACKAGE METHOD NOT FOUND OR IMPLEMENTED");
	}
	//Creation Methods
	private void ReadResponse(CustomCommands.Creation.DoorCreation package)
	{
		HackerDoor.CreateDoor(package);
	}
	private void ReadResponse(CustomCommands.Creation.FireWallCreation package)
	{
		HackerFirewall.CreateFireWall(package);
	}
	private void ReadResponse(CustomCommands.Creation.KeyCardCreation package) { }

	//Updates Methods
	private void ReadResponse(CustomCommands.Update.DoorUpdate package)
	{
		HackerDoor.UpdateDoor(package);
	}
	private void ReadResponse(CustomCommands.Update.FireWallUpdate package)
	{
		HackerFirewall.UpdateFireWall(package);
	}
	private void ReadResponse(CustomCommands.Update.KeyCardUpdate package) { }
	private void ReadResponse(CustomCommands.Update.MinimapUpdate package)
	{
		Texture2D tex = new Texture2D(2, 2);
		tex.LoadImage(package.bytes);
		_minimap.GetComponent<Renderer>().material.mainTexture = tex;
	}
	private void ReadResponse(CustomCommands.Update.PlayerPositionUpdate package)
	{
		MinimapManager.GetInstance().UpdateMinimapPlayer(package);
	}

	//Exit Methods
	private void OnProcessExit(object sender, EventArgs e)
	{
		//_client.Close();
	}
	
	private void OnApplicationQuit()
	{
		//_client.Close();
	}
}



