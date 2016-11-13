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

public class ShooterPackageReader : MonoBehaviour
{

	private TCPMBTesterServer _sender;
	private TcpClient _client;
	private NetworkStream _stream;
	private BinaryFormatter _formatter;
	private DoorManager _doorManager;

	// Use this for initialization
	void Start()
	{
		_sender = GameObject.FindObjectOfType<TCPMBTesterServer>();
		_formatter = new BinaryFormatter();
		_doorManager = GameObject.FindObjectOfType<DoorManager>();
	}

	// Update is called once per frame
	void Update()
	{
		try
		{
			if (_client != null && _client.Available != 0)
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

	private void ReadResponse(CustomCommands.AbstractPackage response)
	{
		if (response is CustomCommands.NotImplementedMessage)
		{
			CustomCommands.NotImplementedMessage NIM = response as CustomCommands.NotImplementedMessage;
			Debug.Log("NotImplemented");
		}
		if (response is CustomCommands.Update.MinimapUpdate)
		{
			//CustomCommands.SendMinimapUpdate MU = response as CustomCommands.SendMinimapUpdate;
			//Texture2D tex = new Texture2D(2, 2);
			//tex.LoadImage(MU.bytes);
			//_minimap.GetComponent<Renderer>().material.mainTexture = tex;
			//Debug.Log("Minimap Updated");
			//_chatBoxScreen.AddChatLine("Minimap Update");
		}
		if (response is CustomCommands.Update.PlayerPositionUpdate)
		{
			//Debug.Log("Updating Player Position");
			//CustomCommands.PlayerPositionUpdate PPU = response as CustomCommands.PlayerPositionUpdate;
			//_minimapManager.UpdateMinimapPlayer(new Vector3(PPU.x, 0, PPU.z));
		}
		if (response is CustomCommands.Creation.DoorCreation)
		{
			//Debug.Log("Updating Door");
			CustomCommands.Creation.DoorCreation DU = response as CustomCommands.Creation.DoorCreation;
			//Debug.Log("Update for Door : " + DU.ID);
			if (!_doorManager.DoorAlreadyExists(DU.ID))
			{
				Debug.Log("-Door does not exists ");
				//_doorManager.CreateDoor(new Vector3(DU.x / _minimapScale.x, 0, DU.z / _minimapScale.y), DU.rotationY, DU.state, DU.ID);
			}
			else
			{
				//Debug.Log("-Door exists");
				_doorManager.UpdateDoor(DU);
			}
		}
		if (response is CustomCommands.Update.DoorUpdate)
		{
			//Debug.Log("Updating Door");
			CustomCommands.Update.DoorUpdate DU = response as CustomCommands.Update.DoorUpdate;
			//Debug.Log("Update for Door : " + DU.ID);
			if (!_doorManager.DoorAlreadyExists(DU.ID))
			{
				Debug.Log("-Door does not exists ");
				//_doorManager.CreateDoor(new Vector3(DU.x / _minimapScale.x, 0, DU.z / _minimapScale.y), DU.rotationY, DU.state, DU.ID);
			}
			else
			{
				//Debug.Log("-Door exists");
				_doorManager.UpdateDoorState(DU);
			}
		}
	}

	public void SetClient(TcpClient client)
	{
		_client = client;
		_stream = client.GetStream();
	}
}