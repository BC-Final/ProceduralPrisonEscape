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
	private HackerFireWallManager _fireWallManager;
	private HackerDoorManager _doorManager;
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

	//Read incoming bytes and returns them when finished
	//private byte[] ReadBytes(int pByteCount)
	//{
	//	byte[] bytes = new byte[pByteCount];
	//	int bytesRead = 0;
	//	int totalBytesRead = 0;
	//
	//	try
	//	{
	//		while (totalBytesRead != pByteCount && (bytesRead = _stream.Read(bytes, totalBytesRead, pByteCount - totalBytesRead)) > 0)
	//		{
	//			totalBytesRead += bytesRead;
	//		}
	//	}
	//	catch
	//	{
	//		Console.WriteLine("Something went wrong");
	//	}
	//
	//	return (totalBytesRead == pByteCount) ? bytes : null;
	//}
	//
	////First gets Message size in bytes then gets Message in bytes itself.
	//private byte[] ReceiveMessage()
	//{
	//	int byteCountToRead = BitConverter.ToInt32(ReadBytes(8), 0);
	//	return ReadBytes(byteCountToRead);
	//}
	//
	////Starts listening to Message with ReceiveMessage(). When finished listening returns message.
	//private string ReceiveString(Encoding pEncoding)
	//{
	//	return pEncoding.GetString(ReceiveMessage());
	//}
	//
	////Starts sending Message size then sends Message itself
	//private void SendMessage(byte[] pMessage)
	//{
	//	_stream.Write(BitConverter.GetBytes(pMessage.Length), 0, 4);
	//	_stream.Write(pMessage, 0, pMessage.Length);
	//}
	//
	////Starts sending a string Message 
	//private void SendString(string pMessage, Encoding pEncoding)
	//{
	//	SendMessage(pEncoding.GetBytes(pMessage));
	//}

	/// <summary>
	/// Sends a custom request to the server
	/// </summary>
	/// <param name="req">Request type</param>
	//private void SendRequest(CustomCommands.AbstractPackage req)
	//{
	//	try
	//	{
	//		BinaryFormatter formatter = new BinaryFormatter();
	//		Debug.Log(req.ToString());
	//		formatter.Serialize(_stream, req);
	//	}
	//	catch (SerializationException e)
	//	{
	//		Console.WriteLine("Failed to serialize. Reason: " + e.Message);
	//		throw;
	//	}
	//
	//}
	/*
	private void ReadResponse(CustomCommands.AbstractPackage response)
	{
		if (response is CustomCommands.NotImplementedMessage)
		{
			CustomCommands.NotImplementedMessage NIM = response as CustomCommands.NotImplementedMessage;
			_chatBoxScreen.AddChatLine(NIM.message);
		}
		if (response is CustomCommands.Update.MinimapUpdate)
		{
			CustomCommands.Update.MinimapUpdate MU = response as CustomCommands.Update.MinimapUpdate;
			Texture2D tex = new Texture2D(2, 2);
			tex.LoadImage(MU.bytes);
			_minimap.GetComponent<Renderer>().material.mainTexture = tex;
			//Debug.Log("Minimap Updated");
			//_chatBoxScreen.AddChatLine("Minimap Update");
		}
		if (response is CustomCommands.Update.PlayerPositionUpdate)
		{
			//Debug.Log("Updating Player Position");
			CustomCommands.Update.PlayerPositionUpdate PPU = response as CustomCommands.Update.PlayerPositionUpdate;
			_minimapManager.UpdateMinimapPlayer(new Vector3(PPU.x, 0, PPU.z), PPU.rotation);
		}
		if (response is CustomCommands.Creation.DoorCreation)
		{
			//Debug.Log("Updating Door");
			CustomCommands.Creation.DoorCreation DU = response as CustomCommands.Creation.DoorCreation;
			//Debug.Log("Update for Door : " + DU.ID);
			if (!_doorManager.DoorAlreadyExists(DU.ID))
			{
				//Debug.Log("-Door does not exists");
				_doorManager.CreateDoor(new Vector3(DU.x/_minimapScale.x, 0, DU.z/_minimapScale.y), DU.rotationY, DU.state, DU.ID);
			}else
			{
				//Debug.Log("-Door exists");
				_doorManager.UpdateDoor(DU);
			}
		}
		if(response is CustomCommands.Update.DoorUpdate)
		{
			CustomCommands.Update.DoorUpdate DCS = response as CustomCommands.Update.DoorUpdate;
			_doorManager.UpdateDoorState(DCS);
		}
		if(response is CustomCommands.Creation.FireWallCreation)
		{
			Debug.Log("Firewall creation request came in");
			CustomCommands.Creation.FireWallCreation FWC = response as CustomCommands.Creation.FireWallCreation;
			Debug.Log("ID array : " + FWC.doorIDs.ToString());
			_fireWallManager.CreateFireWall(FWC);
		}
		if(response is CustomCommands.Update.FireWallUpdate)
		{
			CustomCommands.Update.FireWallUpdate FWU = response as CustomCommands.Update.FireWallUpdate;
			_fireWallManager.UpdateFireWallState(FWU);
		}
	}
	*/
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
		
		//Update Methods
		if (package is CustomCommands.Update.DoorUpdate) { Debug.Log("Package Received : DoorUpdate"); ReadResponse(package as CustomCommands.Update.DoorUpdate); return; }
		if (package is CustomCommands.Update.FireWallUpdate) { Debug.Log("Package Received : FireWallUpdate"); ReadResponse(package as CustomCommands.Update.FireWallUpdate); return; }
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

	//Updates Methods
	private void ReadResponse(CustomCommands.Update.DoorUpdate package)
	{
		HackerDoor.UpdateDoor(package);
	}
	private void ReadResponse(CustomCommands.Update.FireWallUpdate package)
	{
		HackerFirewall.UpdateFireWall(package);
	}
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



