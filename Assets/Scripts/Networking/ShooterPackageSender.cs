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
	private static List<INetworked> _networkObjects = new List<INetworked>();

	private static TcpClient _client;
	private static BinaryFormatter _formatter = new BinaryFormatter();

	public static TcpClient Client {
		get { return _client; }
	}

	public static BinaryFormatter Formatter {
		get { return _formatter; }
	}

	private TcpListener _listener;

	/// <summary>
	/// Adds a networked object to the reference list
	/// </summary>
	/// <param name="pObject"></param>
	public static void RegisterNetworkObject (INetworked pObject) {
		_networkObjects.Add(pObject);
	}
	
	/// <summary>
	/// Removes a networked object from the reference list
	/// </summary>
	/// <param name="pObject"></param>
	public static void UnregisterNetworkedObject (INetworked pObject) {
		_networkObjects.Remove(pObject);
	}

	/// <summary>
	/// Initializes the server
	/// </summary>
	public void Awake () {
		int port = PlayerPrefs.GetInt("HostPort", 55556);
		Debug.Log("Hosted on port : " + port);
		_listener = new TcpListener(IPAddress.Any, port);
		_listener.Start(5);
	}

	/// <summary>
	/// Checks for connecting clients and deletes bad ones
	/// </summary>
	public void Update () {
		CheckForNewClients();
	}

	/// <summary>
	/// 
	/// </summary>
	private void CheckForNewClients () {
		if (_listener.Pending()) {
			TcpClient connectingClient = _listener.AcceptTcpClient();

			if (_client == null) {
				_client = connectingClient;
				Debug.Log(connectingClient.Client.LocalEndPoint.ToString() + " Connected");
				ClientInitialize(connectingClient);
			} else {
				sendPackage(new CustomCommands.RefuseConnectionPackage(), connectingClient);
				connectingClient.GetStream().Close();
				connectingClient.Close();
				Debug.Log("Connecting client refused.");
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="pClient"></param>
	private void ClientInitialize (TcpClient pClient) {
		FindObjectOfType<MinimapCamera>().SendUpdate();

		foreach (INetworked n in _networkObjects) {
			n.Initialize();
		}

		SendPackage(new CustomCommands.Creation.OnCreationEnd());
	}

	/// <summary>
	/// Sends a package to all known clients
	/// </summary>
	/// <param name="package">Sendable data</param>
	public static void SendPackage (CustomCommands.AbstractPackage package) {
		sendPackage(package, _client);
	}

	/// <summary>
	/// Sends a package to all known clients
	/// </summary>
	/// <param name="package">Sendable data</param>
	private static void sendPackage (CustomCommands.AbstractPackage package, TcpClient pClient) {
		if (pClient != null && pClient.Client.Connected) {
			try {
				_formatter.Serialize(pClient.GetStream(), package);
			} catch (Exception e) {
				Debug.LogError("Failed to serialize. Reason: " + e.Message);
			}
		}
	}

	private void OnApplicationQuit () {
		SendPackage(new CustomCommands.ServerShutdownPackage());
		disconnectClient();
	}

	#if UNITY_EDITOR
	void OnDestroy () {
		SendPackage(new CustomCommands.ServerShutdownPackage());
		disconnectClient();
	}
#endif

	public static void SilentlyDisconnect () {
		disconnectClient();
	}

	private static void disconnectClient () {
		if (_client != null) {
			_client.GetStream().Close();
			_client.Close();
			_client = null;
		}
	}
}

