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

public class ShooterPackageSender : MonoBehaviour
{
	private DoorManager _doorManager;
	private FireWallManager _fireWallManager;
	[SerializeField]
	public Camera _camera;
	float _playerPosUpdateTimer = 0;
	float _playerPosUpdateInterval = 0.5f;
	//Player struct with basic player information
	private struct Player
	{
		public Player(string pName, int pScore)
		{
			this.name = pName;
			this.score = pScore;
		}
		public string name;
		public int score;
	}
	//Networking Variables
	private static List<TcpClient> conncectedClients = new List<TcpClient>();
	private static List<TcpClient> clientsToBeDeleted = new List<TcpClient>();
	private static BinaryFormatter formatter = new BinaryFormatter();
	private static ShooterPackageReader _reader;
	private static CustomCommands.AbstractPackage response;
	private TcpListener _listener;
	[SerializeField]
	private Transform _player;

	public void Start()
	{
		Application.runInBackground = true;

		_doorManager = GameObject.FindObjectOfType<DoorManager>();
		_doorManager.SetSender(this);
		_fireWallManager = GameObject.FindObjectOfType<FireWallManager>();
		_fireWallManager.SetSender(this);
		_reader = GameObject.FindObjectOfType<ShooterPackageReader>();
		_listener = new TcpListener(IPAddress.Any, 55556);
		_listener.Start(5);
	}

	public void Update()
	{
		CheckForNewClients();
		UpdateClients();
		DeleteBadClients();
	}

	private void CheckForNewClients()
	{
		if (_listener.Pending())
		{
			//Add new Client
			TcpClient connectingClient = _listener.AcceptTcpClient();
			conncectedClients.Add(connectingClient);
			Debug.Log(connectingClient.Client.LocalEndPoint.ToString() + " Connected");
			ClientInitialize(connectingClient);
		}
	}
	private void UpdateClients()
	{
		_playerPosUpdateTimer += Time.deltaTime;
		if (_playerPosUpdateTimer > _playerPosUpdateInterval)
		{
			foreach (TcpClient client in conncectedClients)
			{

				_playerPosUpdateTimer -= _playerPosUpdateInterval;
				SendResponse(new CustomCommands.Update.PlayerPositionUpdate(_player.position, _player.transform.rotation.eulerAngles.y), client);
			}
		}
	}
	private void DeleteBadClients()
	{
		foreach (TcpClient client in clientsToBeDeleted)
		{
			Debug.Log("Deleting Client");
			conncectedClients.Remove(client);
			DisconnectClient(client);
		}
		clientsToBeDeleted.Clear();
	}

	//Player Management

	private void ClientInitialize(TcpClient pClient)
	{
		SendResponse(new CustomCommands.Update.MinimapUpdate(GetMinimapData()), pClient);
		_reader.SetClient(pClient);
		List<Door> doors = _doorManager.GetDoorList();
		foreach(Door d in doors)
		{
			SendResponse(new CustomCommands.Creation.DoorCreation(d.Id, d.transform.position.x, d.transform.position.z, d.transform.rotation.eulerAngles.y, d.GetDoorState().ToString()), pClient);
		}
		List<Firewall> fireWalls = _fireWallManager._fireWalls;
		foreach(Firewall f in fireWalls)
		{
			List<int> IDs = new List<int>();
			foreach(Door d in f.doors)
			{
				IDs.Add(d.Id);
			}
			int[] doorIDs = IDs.ToArray();
			SendResponse(new CustomCommands.Creation.FireWallCreation(f.ID, f.transform.position.x, f.transform.position.z, f.destroyed, doorIDs), pClient);
		}
	}

	public void SendDoorUpdate(Door door)
	{
		foreach(TcpClient client in conncectedClients)
		{
		SendResponse(new CustomCommands.Update.DoorUpdate(door.Id, door.GetDoorState().ToString()), client);
		}
	}
	public void SendFireWallUpdate(Firewall firewall)
	{
		foreach (TcpClient client in conncectedClients)
		{
			SendResponse(new CustomCommands.Update.FireWallUpdate(firewall.ID, firewall.destroyed), client);
		}
	}
	//Processes the request and returns a response
	private CustomCommands.AbstractPackage GetResponse(CustomCommands.AbstractPackage request, TcpClient client)
	{
		if(request is CustomCommands.MinimapUpdateRequest)
		{
			return new CustomCommands.Update.MinimapUpdate(GetMinimapData());
		}
		if(request is CustomCommands.PlayerPositionUpdateRequest)
		{
			return new CustomCommands.Update.PlayerPositionUpdate(_player.position, _player.rotation.eulerAngles.y);
		}
		return new CustomCommands.NotImplementedMessage();
	}

	//Starts sending a response
	private static void SendResponse(CustomCommands.AbstractPackage response, TcpClient client)
	{
		try
		{
			if (client.Client.Connected)
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(client.GetStream(), response);
			}
			else
			{
				clientsToBeDeleted.Add(client);
			}
		}
		catch (SerializationException e)
		{
			Debug.Log("Failed to serialize. Reason: " + e.Message);
			clientsToBeDeleted.Add(client);
			throw;
		}
	}
	//Connection Management
	private static bool ClientIsConnected(TcpClient client)
	{
		if (client.Client.Poll(0, SelectMode.SelectRead))
		{
			byte[] buff = new byte[1];
			if (client.Client.Receive(buff, SocketFlags.Peek) == 0)
			{
				// Client disconnected
				return false;
			}
			else
			{
				return true;
			}
		}
		return true;
	}


	//Mainly disconnecting Clients
	private static void OnProcessExit(object sender, EventArgs e)
	{
		Console.WriteLine("Im outta here");
		DisconnectAllClients();
	}
	private static void DisconnectClient(TcpClient client)
	{
		Console.WriteLine("Client Disconnected");
		conncectedClients.Remove(client);
		client.GetStream().Close();
		client.Close();
	}
	private static void DisconnectAllClients()
	{
		foreach (TcpClient client in conncectedClients)
		{
			DisconnectClient(client);
		}
	}

	//Internal Methods
	private byte[] GetMinimapData()
	{
		RenderTexture rt = new RenderTexture(512, 512, 24);

		_camera.targetTexture = rt;
		_camera.Render();

		RenderTexture.active = rt;
		Texture2D tex2d = new Texture2D(512, 512, TextureFormat.RGB24, false);
		tex2d.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);

		RenderTexture.active = null;
		_camera.targetTexture = null;

		byte[] bytes;
		bytes = tex2d.EncodeToPNG();

		return bytes;
	}
}

