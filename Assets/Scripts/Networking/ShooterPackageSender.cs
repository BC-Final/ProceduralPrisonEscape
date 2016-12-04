using System;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
using Gamelogic.Extensions;

public class ShooterPackageSender : Singleton<ShooterPackageSender> {
	//Networking Variables
	private static List<TcpClient> _clients = new List<TcpClient>();
	private static List<TcpClient> _clientsToBeDeleted = new List<TcpClient>();
	private static BinaryFormatter _formatter;
	private TcpListener _listener;

	private static List<IShooterNetworked> _networkObjects = new List<IShooterNetworked>();

	public static void RegisterNetworkObject (IShooterNetworked pObject) {
		_networkObjects.Add(pObject);
	}

	public static void UnregisterNetworkedObject (IShooterNetworked pObject) {
		_networkObjects.Remove(pObject);
	}

	public void Start () {
		Application.runInBackground = true;
		_formatter = new BinaryFormatter();
		int port = PlayerPrefs.GetInt("HostPort", 55556);
		Debug.Log("Hosted on port : " + port);
		_listener = new TcpListener(IPAddress.Any, port);
		_listener.Start(5);
	}

	public void Update () {
		CheckForNewClients();
		DeleteBadClients();
	}

	private void CheckForNewClients () {
		if (_listener.Pending()) {
			//Add new Client
			TcpClient connectingClient = _listener.AcceptTcpClient();
			_clients.Add(connectingClient);
			ShooterPackageReader.SetClients(_clients);
			Debug.Log(connectingClient.Client.LocalEndPoint.ToString() + " Connected");
			ClientInitialize(connectingClient);

		}
	}

	private void DeleteBadClients () {
		foreach (TcpClient client in _clientsToBeDeleted) {
			Debug.Log("Deleting Client");
			_clients.Remove(client);
			disconnectClient(client);
		}
		_clientsToBeDeleted.Clear();
	}

	//Player Management
	private void ClientInitialize (TcpClient pClient) {
		FindObjectOfType<MinimapCamera>().SendUpdate();
		//_reader.SetClients(pClient);

		foreach (IShooterNetworked n in _networkObjects) {
			n.Initialize(pClient);
		}
				
		/*
		foreach (ShooterDoor d in ShooterDoor.GetDoorList()) {
			SendPackage(new CustomCommands.Creation.DoorCreation(d.Id, d.transform.position.x, d.transform.position.z, d.transform.rotation.eulerAngles.y, (int)d.GetDoorState()), pClient);
		}

		foreach (ShooterFireWall f in ShooterFireWall.GetFirewallList()) {
			List<int> IDs = new List<int>();

			foreach (ShooterDoor d in f.GetDoorList()) {
				IDs.Add(d.Id);
			}

			int[] doorIDs = IDs.ToArray();
			SendPackage(new CustomCommands.Creation.FireWallCreation(f.Id, f.transform.position.x, f.transform.position.z, f.GetState(), doorIDs), pClient);
		}

		foreach (KeyCard k in KeyCard.GetKeyCardList()) {
			SendPackage(new CustomCommands.Creation.Items.KeyCardCreation(k.Id, k.transform.position.x, k.transform.position.z, false));
		}

		foreach (DummyNode d in DummyNode.GetNodeList()) {
			ShooterPackageSender.SendPackage(new CustomCommands.Creation.NodeCreation(d.GetComponent<RectTransform>().anchoredPosition.x, d.GetComponent<RectTransform>().anchoredPosition.y, d.Id, (int)d.Type, (d.AssociatedObject == null) ? 0 : d.AssociatedObject.GetComponent<INetworked>().Id, d.GetConnections().Select(x => x.Id).ToArray()));
		}
		*/

		SendPackage(new CustomCommands.Creation.OnCreationEnd());
	}

	/// <summary>
	/// Sends a package to all known clients
	/// </summary>
	/// <param name="package">Sendable data</param>
	public static void SendPackage (CustomCommands.AbstractPackage package) {
		foreach (TcpClient client in _clients) {
			SendPackage(package, client);
		}
	}

	/// <summary>
	/// Sends a pckage to a specific client
	/// </summary>
	/// <param name="package">Sendable Data</param>
	/// <param name="client"></param>
	public static void SendPackage (CustomCommands.AbstractPackage package, TcpClient client) {
		try {
			if (client.Client.Connected) {
				_formatter.Serialize(client.GetStream(), package);
			} else {
				_clientsToBeDeleted.Add(client);
			}
		} catch (SerializationException e) {
			Debug.Log("Failed to serialize. Reason: " + e.Message);
			_clientsToBeDeleted.Add(client);
			throw;
		}
	}

	//Connection Management
	private static bool ClientIsConnected (TcpClient client) {
		if (client.Client.Poll(0, SelectMode.SelectRead)) {
			byte[] buff = new byte[1];
			if (client.Client.Receive(buff, SocketFlags.Peek) == 0) {
				// Client disconnected
				return false;
			} else {
				return true;
			}
		}
		return true;
	}

	private static void OnProcessExit (object sender, EventArgs e) {
		foreach (TcpClient client in _clients) {
			disconnectClient(client);
		}
	}

	private static void disconnectClient (TcpClient client) {
		Console.WriteLine("Client Disconnected");
		_clients.Remove(client);
		client.GetStream().Close();
		client.Close();
	}
}

