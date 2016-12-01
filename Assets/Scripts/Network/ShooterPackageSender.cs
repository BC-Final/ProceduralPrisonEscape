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
	private static ShooterPackageSender _instance;
	public static ShooterPackageSender GetInstance()
	{
		if (_instance == null)
		{
			_instance = FindObjectOfType<ShooterPackageSender>();
			if (_instance == null)
			{
				Debug.Log("ERROR!!! PACKAGE SENDER NOT FOUND");
			}
		}
		return _instance;
	}

	[SerializeField]
	public Camera _camera;
	private float _playerPosUpdateTimer = 0;
	[SerializeField]
	private float _playerPosUpdateInterval = 0.5f;
	[SerializeField]
	private Transform _player;

	//Networking Variables
	private static List<TcpClient> _clients = new List<TcpClient>();
	private static List<TcpClient> clientsToBeDeleted = new List<TcpClient>();
	private static BinaryFormatter formatter = new BinaryFormatter();

	private TcpListener _listener;


	public void Start()
	{
		Application.runInBackground = true;
		int port;
		int.TryParse(PlayerPrefs.GetString("HostPort"), out port);
		Debug.Log("Hosted on port : " + port);
		_listener = new TcpListener(IPAddress.Any, port);
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
			_clients.Add(connectingClient);
			ShooterPackageReader.SetClients(_clients);
			Debug.Log(connectingClient.Client.LocalEndPoint.ToString() + " Connected");
			ClientInitialize(connectingClient);
			
        }
	}

	//Updates Player Position. To be moved to own class
	private void UpdateClients()
	{
		_playerPosUpdateTimer += Time.deltaTime;
		if (_playerPosUpdateTimer > _playerPosUpdateInterval)
		{
			foreach (TcpClient client in _clients)
			{

				_playerPosUpdateTimer -= _playerPosUpdateInterval;
				SendPackage(new CustomCommands.Update.PlayerPositionUpdate(_player.position, _player.transform.rotation.eulerAngles.y), client);
			}
		}
	}
	private void DeleteBadClients()
	{
		foreach (TcpClient client in clientsToBeDeleted)
		{
			Debug.Log("Deleting Client");
			_clients.Remove(client);
			DisconnectClient(client);
		}
		clientsToBeDeleted.Clear();
	}

	//Player Management

	private void ClientInitialize(TcpClient pClient)
	{
		SendPackage(new CustomCommands.Update.MinimapUpdate(GetMinimapData()), pClient);
		//_reader.SetClients(pClient);
		foreach(ShooterDoor d in ShooterDoor.GetDoorList())
		{
			SendPackage(new CustomCommands.Creation.DoorCreation(d.Id, d.transform.position.x, d.transform.position.z, d.transform.rotation.eulerAngles.y, d.GetDoorState().ToString()), pClient);
		}
		
		foreach(ShooterFireWall f in ShooterFireWall.GetFirewallList())
		{
			List<int> IDs = new List<int>();
			
			foreach(ShooterDoor d in f.GetDoorList())
			{
				IDs.Add(d.Id);
			}
			
			int[] doorIDs = IDs.ToArray();
			SendPackage(new CustomCommands.Creation.FireWallCreation(f.Id, f.transform.position.x, f.transform.position.z, f.GetState(), doorIDs), pClient);
		}

		foreach(KeyCard k in KeyCard.GetKeyCardList())
		{
			SendPackage(new CustomCommands.Creation.Items.KeyCardCreation(k.Id, k.transform.position.x, k.transform.position.z, false));
		}

		foreach (DummyNode d in DummyNode.GetNodeList()) {
			ShooterPackageSender.SendPackage(new CustomCommands.Creation.NodeCreation(d.GetComponent<RectTransform>().anchoredPosition.x, d.GetComponent<RectTransform>().anchoredPosition.y, d.Id, (int)d.Type, d.AssociatedObject.GetComponent<INetworked>().Id, d.GetConnections().Select(x => x.Id).ToArray()));
		}
	}

	public void SendFireWallUpdate(ShooterFireWall firewall)
	{
		foreach (TcpClient client in _clients)
		{
			SendPackage(new CustomCommands.Update.FireWallUpdate(firewall.Id, firewall.GetState()), client);
		}
	}


	/// <summary>
	/// Sends a package to all known clients
	/// </summary>
	/// <param name="package"></param>
	public static void SendPackage(CustomCommands.AbstractPackage package)
	{
		foreach (TcpClient client in _clients)
		{
			SendPackage(package, client);
		}
	}
	/// <summary>
	/// Sends a pckage to a specific client
	/// </summary>
	/// <param name="package"></param>
	/// <param name="client"></param>
	private static void SendPackage(CustomCommands.AbstractPackage package, TcpClient client)
	{
		try
		{
			if (client.Client.Connected)
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(client.GetStream(), package);
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
		_clients.Remove(client);
		client.GetStream().Close();
		client.Close();
	}
	private static void DisconnectAllClients()
	{
		foreach (TcpClient client in _clients)
		{
			DisconnectClient(client);
		}
	}

	//Internal Methods
	private byte[] GetMinimapData()
	{
		RenderTexture rt = new RenderTexture(1024, 1024, 24);

		_camera.targetTexture = rt;
		_camera.Render();

		RenderTexture.active = rt;
		Texture2D tex2d = new Texture2D(1024, 1024, TextureFormat.RGB24, false);
		tex2d.ReadPixels(new Rect(0, 0, 1024, 1024), 0, 0);

		RenderTexture.active = null;
		_camera.targetTexture = null;

		byte[] bytes;
		bytes = tex2d.EncodeToPNG();

		return bytes;
	}
}

